namespace TC1.RepairShop.Domain.CustomExceptions.BusinessException
{
    public class BusinessException : Exception
    {
        public int StatusCode { get;}

        public BusinessException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
