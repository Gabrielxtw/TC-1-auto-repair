namespace TC1.RepairShop.Domain.CustomError.BusinessErrors
{
    public record BusinessError(int StatusCode, string Message = "");
}