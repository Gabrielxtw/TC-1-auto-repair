namespace TC1.RepairShop.Application
{
    public interface BaseUseCase<TRequest, TResponse>
    {
        Task<BaseResponse<TResponse>> ExecuteAsync(TRequest request);
    }

    public interface BaseUseCase<TResponse>
    {
        Task<BaseResponse<TResponse>> ExecuteAsync();
    }
}
