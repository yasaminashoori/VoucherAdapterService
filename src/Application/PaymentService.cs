using Common.Exceptions;
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
            throw new BusinessException("One or more payment adapters are not registered.", "ADAPTER_NOT_REGISTERED");

        _adapters = new Dictionary<PaymentType, ITarget>
        {
            { PaymentType.Bank, bankAdapter },
            { PaymentType.Cheque, chequeAdapter },
            { PaymentType.Cash, cashAdapter }
        };
    }

    public PaymentResult Process(PaymentType type, Money amount, string? description = null)
    {
        if (!_adapters.TryGetValue(type, out var adapter))
            throw new BusinessException($" {type}: not supported.", "PAYMENT_TYPE_NOT_SUPPORTED");

        return adapter.Process(amount, description);
    }
}

public enum PaymentType
{
    Bank = 1,
    Cheque = 2,
    Cash = 3
}

