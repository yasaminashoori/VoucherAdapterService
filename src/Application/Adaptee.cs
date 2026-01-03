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

public record ChequeRegistrationResult(bool Success, string? TransactionId, string Message);

public class ChequeAdaptee
{
    private readonly string _chequeNumber;
    private readonly DateTime _dueDate;

    public ChequeAdaptee(string chequeNumber, DateTime dueDate)
    {
        _chequeNumber = chequeNumber;
        _dueDate = dueDate;
    }

    public ChequeRegistrationResult RegisterPayment(long tomanAmount, string notes)
    {
        if (_dueDate < DateTime.Now)
            return new ChequeRegistrationResult(false, null, "Cheque has expired");

        var transactionId = $"CHEQUE-{Guid.NewGuid():N}";
        Console.WriteLine($"Cheque Payment: {_chequeNumber} | {tomanAmount:N0} Toman | {notes}");
        
        return new ChequeRegistrationResult(true, transactionId, "Cheque payment registered");
    }
}

public class CashAdaptee
{
    private readonly string _registerId;

    public CashAdaptee(string registerId)
    {
        _registerId = registerId;
    }

    public string RecordTransaction(long tomanAmount, string memo)
    {
        var transactionId = $"CASH-{Guid.NewGuid():N}";
        Console.WriteLine($"Cash Payment: {_registerId} | {tomanAmount:N0} Toman | {memo}");
        return transactionId;
    }
}
