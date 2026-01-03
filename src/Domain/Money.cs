namespace Domain;

public class Money
{
    private const int TomanToRial = 10;
    
    public long Value { get; }
    public Currency Currency { get; }

    public Money(long value, Currency currency)
    {
        Value = value;
        Currency = currency;
    }

    public static Money Rial(long value) => new(value, Currency.Rial);
    public static Money Toman(long value) => new(value, Currency.Toman);
    public static Money Zero => new(0, Currency.Rial);

    public Money ToRial() => Currency == Currency.Rial ? this : new(Value * TomanToRial, Currency.Rial);
    public Money ToToman() => Currency == Currency.Toman ? this : new(Value / TomanToRial, Currency.Toman);
}

public enum Currency
{
    Rial = 1,
    Toman = 2
}

