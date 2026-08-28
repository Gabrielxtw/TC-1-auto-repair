using System.Net;

namespace TC1.RepairShop.Application
{
    public record BaseResponse<T>(T data, bool success = true, string error = "", HttpStatusCode StatusCode = HttpStatusCode.OK);
}
