using Api.Models;
using Application;
using Application.Validators;
using Common.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _service;
    private readonly IPaymentRequestValidator _validator;

    public PaymentController(PaymentService service, IPaymentRequestValidator validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpPost]
    public IActionResult Process([FromBody] PaymentRequest? request)
    {
        var dto = request is null
            ? null
            : new PaymentRequestDto(request.Type, request.Amount, request.Currency, request.Description);

        _validator.Validate(dto);

        var amount = request!.Currency == Currency.Rial
            ? Money.Rial(request.Amount)
            : Money.Toman(request.Amount);

        var result = _service.Process(request.Type, amount, request.Description);

        return Ok(ApiResponse<PaymentResult>.SuccessResponse(result, "Payment processed successfully"));
    }
}
