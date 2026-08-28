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

    protected override async Task<BaseResponse<ListServiceOrdersResponse>> HandleAsync()
    {
        var orders = await serviceOrderRepository.GetAllAsync();
        var dto = ServiceOrdersDTO.ToListServiceOrdersResponse(orders);
        return new BaseResponse<ListServiceOrdersResponse>(dto);
    }
}
