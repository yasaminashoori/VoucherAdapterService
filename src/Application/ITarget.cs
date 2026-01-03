using Domain;

namespace Application;

public interface ITarget
{
    PaymentResult Process(Money amount, string? description = null);
}

public record PaymentResult(string TransactionId, bool Success, string Message);

