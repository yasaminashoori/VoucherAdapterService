using Common.Exceptions;
using Domain;

namespace Application;

public class PaymentService
{
    private readonly Dictionary<PaymentType, ITarget> _adapters;

    public PaymentService(IEnumerable<ITarget> adapters)
    {
        _adapters = adapters.ToDictionary(a => a.PaymentType);

        var missingTypes = Enum.GetValues<PaymentType>()
            .Where(type => !_adapters.ContainsKey(type))
            .ToList();

        if (missingTypes.Any())
        {
            var missing = string.Join(", ", missingTypes);
            throw new BusinessException(
                $"Missing payment adapters: {missing}", 
                "ADAPTER_NOT_REGISTERED");
        }
    }

    public PaymentResult Process(PaymentType type, Money amount, string? description = null)
    {
        if (!_adapters.TryGetValue(type, out var adapter))
        {
            throw new BusinessException(
                $"Payment type '{type}' is not supported.", 
                "PAYMENT_TYPE_NOT_SUPPORTED");
        }

        return adapter.Process(amount, description);
    }
}
