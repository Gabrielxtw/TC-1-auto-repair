using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TC1.RepairShop.Domain.CustomExceptions;

namespace TC1.RepairShop.Domain.ValueObjects
{
    public sealed class Email
    {
        private Email(string value) => Value = value;

        public string Value { get; }

        private static readonly Regex EmailPattern = new(
            "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$",
            RegexOptions.None, TimeSpan.FromSeconds(1));

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !EmailPattern.IsMatch(email))
            {
                throw new BusinessException(BusinessErrors.EmailErrors.InvalidFormat);
            }

            var normalized = email.Trim().ToLowerInvariant();
            return new Email(normalized);
        }
    }
}
