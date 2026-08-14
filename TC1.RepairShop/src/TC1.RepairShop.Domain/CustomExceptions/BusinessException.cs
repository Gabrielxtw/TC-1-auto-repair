namespace TC1.RepairShop.Domain.CustomExceptions
{
    public class BusinessException : Exception
    {
        public int StatusCode { get;}

        public BusinessException(BusinessError error) : base(error.Message)
        {
            StatusCode = error.StatusCode;
        }
    }

    public class BusinessError
    {

        public string Message { get; private set; } = string.Empty;
        public int StatusCode { get; private set; }

        public BusinessError(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
