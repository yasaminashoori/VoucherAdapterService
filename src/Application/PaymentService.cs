using Domain;

namespace Application;

public class PaymentService
{
    private readonly Dictionary<PaymentType, ITarget> _adapters;

    public PaymentService(IEnumerable<ITarget> adapters)
    {
        var bankAdapter = adapters.OfType<BankAdapter>().FirstOrDefault();
        var chequeAdapter = adapters.OfType<ChequeAdapter>().FirstOrDefault();
        var cashAdapter = adapters.OfType<CashAdapter>().FirstOrDefault();

        if (bankAdapter == null || chequeAdapter == null || cashAdapter == null)
            throw new InvalidOperationException("One or more payment adapters are not registered.");

        _adapters = new Dictionary<PaymentType, ITarget>
        {
            { PaymentType.Bank, bankAdapter },
            { PaymentType.Cheque, chequeAdapter },
            { PaymentType.Cash, cashAdapter }
        };
    }

    public PaymentResult Process(PaymentType type, Money amount, string description)
    {
        if (!_adapters.TryGetValue(type, out var adapter))
            throw new NotSupportedException($"Payment type {type} is not supported.");

        return adapter.Process(amount, description);
    }
}

public enum PaymentType
{
    Bank = 1,
    Cheque = 2,
    Cash = 3
}

