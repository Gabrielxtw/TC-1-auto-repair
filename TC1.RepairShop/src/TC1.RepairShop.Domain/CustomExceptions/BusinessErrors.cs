using TC1.RepairShop.Domain.CustomError.BusinessErrors;

namespace TC1.RepairShop.Domain.CustomExceptions
{
    public static class BusinessErrors
    {
        public static class RequestErrors
        {
            public static readonly BusinessError NotFound = new(
                Message: "Not Found",
                StatusCode: 404);
        }
        public static class EntityErrors
        {
            public static readonly BusinessError CannotDoActionInactiveEntity = new(
                Message: "Cannot do action on an inactive entity.",
                StatusCode: 400);
        }

        public static class DocumentErrors
        {
            public static readonly BusinessError InvalidFormat = new(
                Message: "The document value must be a valid CPF or CNPJ.",
                StatusCode: 400);
        }
        public static class EmailErrors
        {
            public static readonly BusinessError InvalidFormat = new(
                Message: "The email value must be a valid email address.",
                StatusCode: 400);
        }

        public static class LicensePlateErrors
        {
            public static readonly BusinessError InvalidFormat = new(
                Message: "The license plate value must be a valid Brazilian license plate.",
                StatusCode: 400);
        }

        public static class PartErrors
        {
            public static readonly BusinessError CannotAlterStockFromInactivePart = new(
                Message: "Cannot alter stock from an inactive part.",
                StatusCode: 400);
        }

        public static class ServiceOrderErrors
        {
            public static readonly BusinessError InvalidStatusTransition = new(
                Message: "Cannot transition from the current status to the new status.",
                StatusCode: 400);

            public static readonly BusinessError QuantityMustBePositive = new(
                Message: "Quantity must be greater than zero.",
                StatusCode: 400);

            public static readonly BusinessError ServiceAlreadyRegistered = new(
                Message: "Service is already registered for this order.",
                StatusCode: 400);

            public static readonly BusinessError PartAlreadyRegistered = new(
                Message: "Part is already registered for this order.",
                StatusCode: 400);
        }

        public static class QuoteErrors
        {
            public static readonly BusinessError CannotRejectApprovedQuote = new(
                Message: "Cannot reject an approved quote.",
                StatusCode: 400);

            public static readonly BusinessError MaxRejectionCount= new(
                Message: "Quote has reached the maximum rejection count.",
                StatusCode: 400);
        }
    }
}