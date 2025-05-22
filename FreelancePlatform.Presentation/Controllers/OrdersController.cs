using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FreelancePlatform.Application.Services;
using FreelancePlatform.Application.DesignPatterns.FactoryMethod;
using FreelancePlatform.Application.DesignPatterns.Decorator;
using FreelancePlatform.Application.DesignPatterns.Observer;
using FreelancePlatform.Application.DesignPatterns.Facade;
using FreelancePlatform.Application.DTOs;
using System.Security.Claims;
using FreelancePlatform.Domain.Interfaces;

namespace FreelancePlatform.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly Dictionary<string, IOrderFactory> _factories;
    private readonly OrderService _orderService;
    private readonly NotificationService _notificationService;
    private readonly OrderFacade _orderFacade;

    public OrdersController(OrderService orderService, NotificationService notificationService, OrderFacade orderFacade)
    {
        _orderService = orderService;
        _notificationService = notificationService;
        _orderFacade = orderFacade;
        _factories = new Dictionary<string, IOrderFactory>
        {
            { "design", new DesignOrderFactory() },
            { "coding", new CodingOrderFactory() },
            { "marketing", new MarketingOrderFactory() }
        };
        _notificationService.Subscribe(new EmailNotifier());
    }

    [HttpPost("create/{type}")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CreateOrder(string type, [FromBody] CreateOrderRequest request)
    {
        if (!_factories.TryGetValue(type.ToLower(), out var factory))
            return BadRequest("Invalid order type");

        var order = await _orderService.CreateOrderAsync(request.Title, request.BasePrice, User.FindFirst(ClaimTypes.NameIdentifier)?.Value, factory, _notificationService);
        return Ok(order);
    }

    [HttpPost("clone/{orderId}")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CloneOrder(string orderId)
    {
        var order = await _orderService.CloneOrderAsync(orderId, _notificationService);
        return Ok(order);
    }

    [HttpGet("active")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetActiveOrders()
    {
        var orders = await _orderService.GetActiveOrdersAsync();
        return Ok(orders);
    }

    [HttpPost("enhance/{orderId}")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> EnhanceOrder(string orderId, [FromBody] EnhanceOrderRequest request)
    {
        var order = await _orderService.EnhanceOrderAsync(orderId, request.IsUrgent, request.HasPremiumSupport);
        IOrderComponent component = new BasicOrder(new Domain.Entities.Order { Title = order.Title, BasePrice = order.BasePrice });
        if (order.IsUrgent)
            component = new UrgentOrderDecorator(component);
        if (order.HasPremiumSupport)
            component = new PremiumSupportDecorator(component);
        return Ok(new { order, component.Description, component.Cost });
    }
}

public record CreateOrderRequest(string Title, decimal BasePrice);
public record EnhanceOrderRequest(bool IsUrgent, bool HasPremiumSupport);