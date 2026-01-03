using Domain;

namespace Application;

public class BankAdapter : ITarget
{
    private readonly BankAdaptee _bankAdaptee;

    public BankAdapter(BankAdaptee bankAdaptee)
    {
        _bankAdaptee = bankAdaptee;
    }

    public PaymentType PaymentType => PaymentType.Bank;

    public PaymentResult Process(Money amount, string? description = null)
    {
        var rialAmount = amount.ToRial().Value;
        var transactionId = _bankAdaptee.ExecuteTransfer(rialAmount, description ?? string.Empty);
        return new PaymentResult(transactionId, true, "Bank payment processed");
    }
}

public class ChequeAdapter : ITarget
{
    private readonly ChequeAdaptee _chequeAdaptee;

    public ChequeAdapter(ChequeAdaptee chequeAdaptee)
    {
        _chequeAdaptee = chequeAdaptee;
    }

    public PaymentType PaymentType => PaymentType.Cheque;

    public PaymentResult Process(Money amount, string? description = null)
    {
        var tomanAmount = amount.ToToman().Value;  // Now using long, no truncation
        var result = _chequeAdaptee.RegisterPayment(tomanAmount, description ?? string.Empty);
        
        return new PaymentResult(
            result.TransactionId ?? string.Empty, 
            result.Success, 
            result.Message);
    }
}

public class CashAdapter : ITarget
{
    private readonly CashAdaptee _cashAdaptee;

    public CashAdapter(CashAdaptee cashAdaptee)
    {
        _cashAdaptee = cashAdaptee;
    }

    public PaymentType PaymentType => PaymentType.Cash;

    public PaymentResult Process(Money amount, string? description = null)
    {
        var tomanAmount = amount.ToToman().Value;
        var transactionId = _cashAdaptee.RecordTransaction(tomanAmount, description ?? string.Empty);
        return new PaymentResult(transactionId, true, "Cash payment recorded");
    }
}
