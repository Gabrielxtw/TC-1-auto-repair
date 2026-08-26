using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TC1.RepairShop.Domain.CustomExceptions;

namespace TC1.RepairShop.Domain.ValueObjects
{
    public sealed class Document
    {
        private Document(string value) => Value = value;
        public string Value { get; }

        public static Document Create(string value)
        {
            if (!IsValidDocument(value))
                throw new BusinessException(BusinessErrors.DocumentErrors.InvalidFormat);
            return new Document(value.Trim());
        }

        private static readonly TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

        private static readonly Regex CpfPattern = new(
            "^\\d{3}\\.?\\d{3}\\.?\\d{3}-?\\d{2}$", RegexOptions.None, RegexTimeout);

        private static readonly Regex CnpjPattern = new(
            "^\\d{2}\\.?\\d{3}\\.?\\d{3}/?\\d{4}-?\\d{2}$", RegexOptions.None, RegexTimeout);

        private static bool IsValidDocument(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (CpfPattern.IsMatch(value)) //CPF pattern
                return true;
            else if (CnpjPattern.IsMatch(value)) //CNPJ pattern
                return true;
            return false;
        }
    }
}
