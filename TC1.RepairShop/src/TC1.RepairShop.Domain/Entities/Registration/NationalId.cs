namespace TC1.RepairShop.Domain.Entities.Registration;

public sealed class NationalId
{
    public string Value { get; }

    private NationalId(string value)
    {
        Value = value;
    }

    public static NationalId Create(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException($"Invalid CPF/CNPJ: '{value}'.", nameof(value));
        }

        return new NationalId(OnlyDigits(value));
    }

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var digits = OnlyDigits(value);

        return digits.Length switch
        {
            11 => IsValidCpf(digits),
            14 => IsValidCnpj(digits),
            _ => false,
        };
    }

    private static string OnlyDigits(string value) =>
        new(value.Where(char.IsDigit).ToArray());

    private static bool IsValidCpf(string cpf)
    {
        if (AllDigitsEqual(cpf))
        {
            return false;
        }

        var digits = cpf.Select(c => c - '0').ToArray();

        var firstCheckDigit = CalculateCheckDigit(digits, 9, initialMultiplier: 10);
        if (firstCheckDigit != digits[9])
        {
            return false;
        }

        var secondCheckDigit = CalculateCheckDigit(digits, 10, initialMultiplier: 11);
        return secondCheckDigit == digits[10];
    }

    private static bool IsValidCnpj(string cnpj)
    {
        if (AllDigitsEqual(cnpj))
        {
            return false;
        }

        var digits = cnpj.Select(c => c - '0').ToArray();

        int[] firstDigitWeights = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
        int[] secondDigitWeights = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

        var firstCheckDigit = CalculateWeightedCheckDigit(digits, 12, firstDigitWeights);
        if (firstCheckDigit != digits[12])
        {
            return false;
        }

        var secondCheckDigit = CalculateWeightedCheckDigit(digits, 13, secondDigitWeights);
        return secondCheckDigit == digits[13];
    }

    private static int CalculateCheckDigit(int[] digits, int count, int initialMultiplier)
    {
        var sum = 0;
        var multiplier = initialMultiplier;

        for (var i = 0; i < count; i++)
        {
            sum += digits[i] * multiplier;
            multiplier--;
        }

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }

    private static int CalculateWeightedCheckDigit(int[] digits, int count, int[] weights)
    {
        var sum = 0;
        for (var i = 0; i < count; i++)
        {
            sum += digits[i] * weights[i];
        }

        var remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }

    private static bool AllDigitsEqual(string value) => value.Distinct().Count() == 1;

    public override string ToString() => Value;
}
