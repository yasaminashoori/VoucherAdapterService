using Application;
using Api.Models;
using Common.Exceptions;
using Common.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _service;

    public PaymentController(PaymentService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Process([FromBody] PaymentRequest request)
    {
        if (request == null)
            throw new ValidationException("Request body is required.");

        if (request.Amount <= 0)
            throw new ValidationException("Amount must be greater than zero.");

        var amount = request.Currency == Currency.Rial
            ? Money.Rial(request.Amount)
            : Money.Toman(request.Amount);

        var result = _service.Process(request.Type, amount, request.Description);
        return Ok(ApiResponse<PaymentResult>.SuccessResponse(result, "Payment processed successfully"));
    }
}

