using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Interfaces;
using TC1.RepairShop.Application.ServiceOrders.UseCases;

namespace TC1.RepairShop.Application.ServiceOrders.UseCases;

public record ListServiceOrdersRequest(ServiceOrderStatus? Status, DateTime? OpenedFrom, DateTime? OpenedTo, bool Descending = true);

public class ListServiceOrdersUseCase(IServiceOrderRepository serviceOrderRepository) : BaseUseCase<ListServiceOrdersRequest, ListServiceOrdersResponse>
{

    protected override async Task<BaseResponse<ListServiceOrdersResponse>> HandleAsync(ListServiceOrdersRequest request)
    {
        var orders = await serviceOrderRepository.GetFilteredAsync(request.Status, request.OpenedFrom, request.OpenedTo, request.Descending);
        var dto = ServiceOrdersDTO.ToListServiceOrdersResponse(orders);
        return new BaseResponse<ListServiceOrdersResponse>(dto);
    }
}
