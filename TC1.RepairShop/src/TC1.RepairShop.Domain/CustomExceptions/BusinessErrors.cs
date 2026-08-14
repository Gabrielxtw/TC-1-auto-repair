using TC1.RepairShop.Domain.CustomError.BusinessErrors;

namespace TC1.RepairShop.Domain.CustomExceptions
{
    public static class BusinessErrors
    {
        public static class Document
        {
            public static readonly BusinessError InvalidFormat = new(
                Message: "The document value must be a valid CPF or CNPJ.",
                StatusCode: 400);
        }
        public static class Email
        {
            public static readonly BusinessError InvalidFormat = new(
                Message: "The email value must be a valid email address.",
                StatusCode: 400);
        }

        public static class Entity
        {
            public static readonly BusinessError CannotDeactivateInactiveEntity = new(
                Message: "Cannot deactivate an inactive entity.",
                StatusCode: 400);
        }

        public static class LicensePlate
        {
            public static readonly BusinessError InvalidFormat = new(
                Message: "The license plate value must be a valid Brazilian license plate.",
                StatusCode: 400);
        }
    }
}