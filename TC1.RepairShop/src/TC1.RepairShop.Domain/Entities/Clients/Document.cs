using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TC1.RepairShop.Domain.Entities.CustomExceptions;

namespace TC1.RepairShop.Domain.Entities.Clients
{
    public sealed class Document
    {
        private Document(string value) => Value = value;
        public string Value { get; }

        public static Document Create(string value)
        {
            if (!IsValidDocument(value))
                throw new BusinessException(BusinessErrors.Document.InvalidFormat);
            return new Document(value.Trim());
        }

        private static bool IsValidDocument(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (Regex.IsMatch(value, "^\\d{3}\\.?\\d{3}\\.?\\d{3}-?\\d{2}$")) //CPF pattern
                return true;
            else if (Regex.IsMatch(value, "^\\d{2}\\.?\\d{3}\\.?\\d{3}/?\\d{4}-?\\d{2}$")) //CNPJ pattern
                return true;
            return false;
        }
    }
}
