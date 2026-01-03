namespace Domain;

public class Money
{
    private const int TomanToRial = 10;

    public long Value { get; }
    public Currency Currency { get; }

    private Money(long value, Currency currency)
    {
        if (value < 0)
            throw new ArgumentException("Money value cannot be negative.", nameof(value));

        Value = value;
        Currency = currency;
    }

    public static Money Rial(long value) => new(value, Currency.Rial);
    public static Money Toman(long value) => new(value, Currency.Toman);
    public static Money Zero => new(0, Currency.Rial);

    public Money ToRial() => Currency == Currency.Rial
        ? this
        : new(Value * TomanToRial, Currency.Rial);

    public Money ToToman() => Currency == Currency.Toman
        ? this
        : new(Value / TomanToRial, Currency.Toman);
    public long GetRialRemainder() => Currency == Currency.Rial
        ? Value % TomanToRial
        : 0;

    public bool HasRemainderInToman() => GetRialRemainder() > 0;

    public override string ToString() => $"{Value:N0} {Currency}";

    public override bool Equals(object? obj) =>
        obj is Money other && ToRial().Value == other.ToRial().Value;

    public override int GetHashCode() => ToRial().Value.GetHashCode();
}

public enum Currency
{
    Rial = 1,
    Toman = 2
}
