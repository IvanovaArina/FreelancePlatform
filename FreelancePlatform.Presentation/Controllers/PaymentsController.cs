using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FreelancePlatform.Application.DesignPatterns.Facade;
using System.Security.Claims;

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
        var clientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Извлекаем clientId из токена
        if (string.IsNullOrEmpty(clientId))
            return Unauthorized("Client ID not found in token.");
        var (order, transaction) = await _orderFacade.CreateAndPayOrderAsync(type, request.Title, request.BasePrice, clientId, request.Hours, paymentType);
        return Ok(new { Order = order, Transaction = transaction });
    }
}

public record CreateAndPayOrderRequest(string Title, decimal BasePrice, int Hours);