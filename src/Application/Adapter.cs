using Domain;

namespace Application;

public class BankAdapter(BankAdaptee bankAdaptee) : ITarget
{
    public PaymentResult Process(Money amount, string description)
    {
        var rialAmount = amount.ToRial().Value;
        var transactionId = bankAdaptee.ExecuteTransfer(rialAmount, description);
        return new PaymentResult(transactionId, true, "Bank payment processed");
    }
}

public class ChequeAdapter(ChequeAdaptee chequeAdaptee) : ITarget
{
    public PaymentResult Process(Money amount, string description)
    {
        var tomanAmount = (int)amount.ToToman().Value;
        var success = chequeAdaptee.RegisterPayment(tomanAmount, description);
        
        if (!success)
            return new PaymentResult(string.Empty, false, "Cheque expired");

        var transactionId = $"CHEQUE-{Guid.NewGuid():N}";
        return new PaymentResult(transactionId, true, "Cheque payment registered");
    }
}

public class CashAdapter(CashAdaptee cashAdaptee) : ITarget
{
    public PaymentResult Process(Money amount, string description)
    {
        var tomanAmount = (decimal)amount.ToToman().Value;
        cashAdaptee.RecordTransaction(tomanAmount, description);
        var transactionId = $"CASH-{Guid.NewGuid():N}";
        return new PaymentResult(transactionId, true, "Cash payment recorded");
    }
}

