using System;
using System.Collections.Generic;
using System.Text;

namespace TC1.RepairShop.Domain.Entities.CustomExceptions
{
    public static class BusinessErrors
    {
        public static class Document
        {
            public static readonly BusinessError InvalidFormat = new(
                "The document value must be a valid CPF or CNPJ.",
                400);
        }
        public static class Email
        {
            public static readonly BusinessError InvalidFormat = new(
                "The email value must be a valid email address.",
                400);
        }
    }
}
