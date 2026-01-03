using Domain;

namespace Application;

public interface ITarget
{
    PaymentResult Process(Money amount, string description);
}

public record PaymentResult(string TransactionId, bool Success, string Message);

