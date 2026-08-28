using System.Net;
using TC1.RepairShop.Domain.CustomError.BusinessErrors;

namespace TC1.RepairShop.Domain.CustomExceptions
{
    public static class BusinessErrors
    {
        public static class EntityErrors
        {
            public static readonly BusinessError CannotDoActionInactiveEntity = new(
                Message: "Cannot do action on an inactive entity.",
                StatusCode: HttpStatusCode.BadRequest);
            public static readonly BusinessError NotFound = new(
                Message: "Not Found",
                StatusCode: HttpStatusCode.NotFound);
        }

        public static class DocumentErrors
        {
            public static readonly BusinessError InvalidFormat = new(
                Message: "The document value must be a valid CPF or CNPJ.",
                StatusCode: HttpStatusCode.BadRequest);
        }
        public static class EmailErrors
        {
            public static readonly BusinessError InvalidFormat = new(
                Message: "The email value must be a valid email address.",
                StatusCode: HttpStatusCode.BadRequest);
        }

        public static class LicensePlateErrors
        {
            public static readonly BusinessError InvalidFormat = new(
                Message: "The license plate value must be a valid Brazilian license plate.",
                StatusCode: HttpStatusCode.BadRequest);

            public static readonly BusinessError DuplicateLicensePlate = new(
                Message: "License plate is already registered.",
                StatusCode: HttpStatusCode.Conflict);
        }

        public static class VehicleErrors
        {
            public static readonly BusinessError NotFound = new(
                Message: "Vehicle not found.",
                StatusCode: HttpStatusCode.NotFound);

        }

        public static class PartErrors
        {
            public static readonly BusinessError CannotAlterStockFromInactivePart = new(
                Message: "Cannot alter stock from an inactive part.",
                StatusCode: HttpStatusCode.BadRequest);
            public static readonly BusinessError DuplicatePart = new(
                Message: "Part is already registered.",
                StatusCode: HttpStatusCode.Conflict);
            public static readonly BusinessError NotFound = new(
                Message: "Part not found.",
                StatusCode: HttpStatusCode.NotFound);
        }

        public static class ServiceOrderErrors
        {
            public static readonly BusinessError InvalidStatusTransition = new(
                Message: "Cannot transition from the current status to the new status.",
                StatusCode: HttpStatusCode.BadRequest);

            public static readonly BusinessError QuantityMustBePositive = new(
                Message: "Quantity must be greater than zero.",
                StatusCode: HttpStatusCode.BadRequest);

            public static readonly BusinessError ServiceAlreadyRegistered = new(
                Message: "Service is already registered for this order.",
                StatusCode: HttpStatusCode.BadRequest);

            public static readonly BusinessError PartAlreadyRegistered = new(
                Message: "Part is already registered for this order.",
                StatusCode: HttpStatusCode.BadRequest);
            public static readonly BusinessError NotFound = new(
                Message: "Service order not found.",
                StatusCode: HttpStatusCode.NotFound);
        }

        public static class QuoteErrors
        {
            public static readonly BusinessError CannotRejectApprovedQuote = new(
                Message: "Cannot reject an approved quote.",
                StatusCode: HttpStatusCode.BadRequest);

            public static readonly BusinessError MaxRejectionCount= new(
                Message: "Quote has reached the maximum rejection count.",
                StatusCode: HttpStatusCode.BadRequest);
            public static readonly BusinessError NotFound = new(
                Message: "Quote not found.",
                StatusCode: HttpStatusCode.NotFound);
        }

        public static class UserErrors
        {
            public static readonly BusinessError DuplicateUsername = new(
                Message: "Username is already taken.",
                StatusCode: HttpStatusCode.Conflict);
            public static readonly BusinessError NotFound = new(
                Message: "User not found.",
                StatusCode: HttpStatusCode.NotFound);
        }

        public static class ServiceErrors
        {
            public static readonly BusinessError DuplicateService = new(
                Message: "Service is already registered.",
                StatusCode: HttpStatusCode.Conflict);
            public static readonly BusinessError NotFound = new(
                Message: "Service not found.",
                StatusCode: HttpStatusCode.NotFound);
        }
    }
}