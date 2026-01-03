using Common.Exceptions;
using Domain;

namespace Application.Validators;

public record PaymentRequestDto(
    PaymentType Type,
    long Amount,
    Currency Currency,
    string? Description = null
);

public interface IPaymentRequestValidator
{
    void Validate(PaymentRequestDto? request);
}

public class PaymentRequestValidator : IPaymentRequestValidator
{
    public void Validate(PaymentRequestDto? request)
    {
        if (request is null)
            throw new ValidationException("Request body is required.");

        if (request.Amount <= 0)
            throw new ValidationException("Amount must be greater than zero.");

        if (!Enum.IsDefined(request.Type))
            throw new ValidationException($"Invalid payment type: {(int)request.Type}");

        if (!Enum.IsDefined(request.Currency))
            throw new ValidationException($"Invalid currency: {(int)request.Currency}");

        const long MaxTransactionAmount = 10_000_000_000;
        var rialAmount = request.Currency == Currency.Toman
            ? request.Amount * 10
            : request.Amount;

        if (rialAmount > MaxTransactionAmount)
            throw new ValidationException($"Amount exceeds maximum transaction limit.");
    }
}
