using System.Net;
using TC1.RepairShop.Domain.CustomExceptions;

namespace TC1.RepairShop.Application
{
    public interface UseCaseBase<TRequest, TResponse>
    {
        public Task<BaseResponse<TResponse>> ExecuteAsync(TRequest request);
    }

    public interface UseCaseBase<TResponse>
    {
        Task<BaseResponse<TResponse>> ExecuteAsync();
    }

    public abstract class BaseUseCase<TRequest, TResponse> : UseCaseBase<TRequest, TResponse>
    {
        public async Task<BaseResponse<TResponse>> ExecuteAsync(TRequest request)
        {
            try
            {
                return await HandleAsync(request);
            }
            catch (BusinessException ex)
            {
                return new BaseResponse<TResponse>(default!, success: false, error: ex.Message, StatusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TResponse>(default!, success: false, error: ex.Message, StatusCode: HttpStatusCode.InternalServerError);
            }
        }

        protected abstract Task<BaseResponse<TResponse>> HandleAsync(TRequest request);
    }

    public abstract class BaseUseCase<TResponse> : UseCaseBase<TResponse>
    {
        public async Task<BaseResponse<TResponse>> ExecuteAsync()
        {
            try
            {
                return await HandleAsync();
            }
            catch (BusinessException ex)
            {
                return new BaseResponse<TResponse>(default!, success: false, error: ex.Message, StatusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                return new BaseResponse<TResponse>(default!, success: false, error: ex.Message, StatusCode: HttpStatusCode.InternalServerError);
            }
        }

        protected abstract Task<BaseResponse<TResponse>> HandleAsync();
    }
}
