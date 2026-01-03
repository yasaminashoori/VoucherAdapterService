using Domain;

namespace Application;

public interface ITarget
{
    PaymentType PaymentType { get; }
    
    PaymentResult Process(Money amount, string? description = null);
}

public record PaymentResult(string TransactionId, bool Success, string Message);

public enum PaymentType
{
    Bank = 1,
    Cheque = 2,
    Cash = 3
}
