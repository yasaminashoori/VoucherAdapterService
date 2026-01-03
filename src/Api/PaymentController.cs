using Application;
using Api.Models;
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
            return BadRequest(new { error = "Request body is required." });

        if (request.Amount <= 0)
            return BadRequest(new { error = "Amount must be greater than zero." });

        if (string.IsNullOrWhiteSpace(request.Description))
            return BadRequest(new { error = "Description is required." });

        if (string.IsNullOrWhiteSpace(request.Currency))
            return BadRequest(new { error = "Currency is required." });

        try
        {
            var amount = request.Currency.Equals("Rial", StringComparison.OrdinalIgnoreCase)
                ? Money.Rial(request.Amount)
                : request.Currency.Equals("Toman", StringComparison.OrdinalIgnoreCase)
                    ? Money.Toman(request.Amount)
                    : throw new ArgumentException($"Unsupported currency: {request.Currency}");

            var result = _service.Process(request.Type, amount, request.Description);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (NotSupportedException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { error = "An error occurred while processing the payment." });
        }
    }
}

