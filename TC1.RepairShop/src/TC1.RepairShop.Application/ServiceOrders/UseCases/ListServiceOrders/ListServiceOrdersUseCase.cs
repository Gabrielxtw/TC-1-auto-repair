using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Application.ServiceOrders.UseCases;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public class ListServiceOrdersUseCase(IServiceOrderRepository serviceOrderRepository): BaseUseCase<ListServiceOrdersResponse>
{

    public async Task<BaseResponse<ListServiceOrdersResponse>> ExecuteAsync()
    {
        try
        {
            var orders = await serviceOrderRepository.GetAllAsync();
            var dto = ServiceOrdersDTO.ToListServiceOrdersResponse(orders);
            return new BaseResponse<ListServiceOrdersResponse>(dto);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ListServiceOrdersResponse>(new ListServiceOrdersResponse(Enumerable.Empty<ServiceOrderListResponse>()), success: false, error: ex.Message);
        }
    }
}
