using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TC1.RepairShop.Domain.Entities.CustomExceptions;

namespace TC1.RepairShop.Domain.Entities.Clients
{
    public sealed class Email
    {
        private Email(string value) => Value = value;

        public string Value { get; }

        public static Email Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"))
            {
                throw new BusinessException(BusinessErrors.Email.InvalidFormat);
            }

            var normalized = email.Trim().ToLowerInvariant();
            return new Email(normalized);
        }
    }
}
