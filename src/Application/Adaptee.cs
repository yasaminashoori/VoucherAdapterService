using Domain;

namespace Application;

public class BankAdaptee
{
    private readonly string _accountNumber;

    public BankAdaptee(string accountNumber)
    {
        _accountNumber = accountNumber;
    }

    public string ExecuteTransfer(long rialAmount, string memo)
    {
        var transactionId = $"BANK-{Guid.NewGuid():N}";
        Console.WriteLine($"Bank Transfer: {_accountNumber} | {rialAmount:N0} Rial | {memo}");
        return transactionId;
    }
}

public class ChequeAdaptee
{
    private readonly string _chequeNumber;
    private readonly DateTime _dueDate;

    public ChequeAdaptee(string chequeNumber, DateTime dueDate)
    {
        _chequeNumber = chequeNumber;
        _dueDate = dueDate;
    }

    public bool RegisterPayment(int tomanAmount, string notes)
    {
        if (_dueDate < DateTime.Now)
            return false;

        Console.WriteLine($"Cheque Payment: {_chequeNumber} | {tomanAmount:N0} Toman | {notes}");
        return true;
    }
}

public class CashAdaptee
{
    private readonly string _registerId;

    public CashAdaptee(string registerId)
    {
        _registerId = registerId;
    }

    public void RecordTransaction(decimal amount, string memo)
    {
        Console.WriteLine($"Cash Payment: {_registerId} | {amount:N0} | {memo}");
    }
}

