using System.Text.RegularExpressions;

namespace TC1.RepairShop.Domain.Registration;

public sealed partial class LicensePlate
{
    public string Value { get; }

    private LicensePlate(string value)
    {
        Value = value;
    }

    public static LicensePlate Create(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException($"Invalid license plate: '{value}'.", nameof(value));
        }

        return new LicensePlate(Normalize(value));
    }

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalized = Normalize(value);

        return LegacyFormatRegex().IsMatch(normalized) || MercosulFormatRegex().IsMatch(normalized);
    }

    private static string Normalize(string value) =>
        new string(value.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();

    [GeneratedRegex("^[A-Z]{3}[0-9]{4}$")]
    private static partial Regex LegacyFormatRegex();

    [GeneratedRegex("^[A-Z]{3}[0-9][A-Z][0-9]{2}$")]
    private static partial Regex MercosulFormatRegex();

    public override string ToString() => Value;
}
