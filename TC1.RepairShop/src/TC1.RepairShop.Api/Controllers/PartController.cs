using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Parts.UseCases;
using Microsoft.AspNetCore.Authorization;

namespace TC1.RepairShop.Api.Controllers
{
    [Authorize(Policy = "StaffOnly")]
    public class PartController(
        DeactivatePartUseCase _deactivatePartUseCase,
        DeletePartUseCase _deletePartUseCase,
        GetAllPartUseCase _getAllPartUseCase,
        GetPartByIdUseCase _getPartByIdUseCase,
        ReceiveStockUseCase _receiveStockUseCase,
        CreatePartUseCase _registerPartUseCase
        ) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _getAllPartUseCase.ExecuteAsync();
            return Response(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _getPartByIdUseCase.ExecuteAsync(id);
            return Response(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePartRequest request)
        {
            var result = await _registerPartUseCase.ExecuteAsync(request);
            if (!result.success)
            {
                return Conflict(new { message = result.error });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.data?.Id }, result.data);
        }

        [HttpPut("ReceiveStock")]
        public async Task<IActionResult> ReceiveStock([FromBody] ReceiveStockRequest request)
        {
            var result = await _receiveStockUseCase.ExecuteAsync(request);
            return Response(result);
        }

        [HttpPut("Deactive")]
        public async Task<IActionResult> Deactive([FromBody] DeactivePartRequest request)
        {
            var result = await _deactivatePartUseCase.ExecuteAsync(request);
            return Response(result);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _deletePartUseCase.ExecuteAsync(id);
            return Response(result);
        }
    }
}
