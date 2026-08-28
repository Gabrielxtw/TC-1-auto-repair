using System.Net;
using TC1.RepairShop.Domain.CustomError.BusinessErrors;

namespace TC1.RepairShop.Domain.CustomExceptions
{
    public class BusinessException : Exception
    {
        public HttpStatusCode StatusCode { get;}

        public BusinessException(BusinessError error) : base(error.Message)
        {
            StatusCode = error.StatusCode;
        }
    }
}
