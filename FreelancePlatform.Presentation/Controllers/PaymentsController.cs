using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FreelancePlatform.Application.DesignPatterns.Facade;

namespace FreelancePlatform.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly OrderFacade _orderFacade;

    public PaymentsController(OrderFacade orderFacade)
    {
        _orderFacade = orderFacade;
    }

    [HttpPost("create-and-pay/{type}/{paymentType}")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> CreateAndPayOrder(string type, string paymentType, [FromBody] CreateAndPayOrderRequest request)
    {
        var (order, transaction) = await _orderFacade.CreateAndPayOrderAsync(type, request.Title, request.BasePrice, request.ClientId, request.Hours, paymentType);
        return Ok(new { Order = order, Transaction = transaction });
    }
}

public record CreateAndPayOrderRequest(string Title, decimal BasePrice, string ClientId, int Hours);