using System.Net;

namespace TC1.RepairShop.Domain.CustomError.BusinessErrors
{
    public record BusinessError(HttpStatusCode StatusCode, string Message = "");
}