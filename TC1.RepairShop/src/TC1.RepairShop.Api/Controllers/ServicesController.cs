using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TC1.RepairShop.Application.Services.UseCases;

namespace TC1.RepairShop.Api.Controllers
{
    [Authorize(Policy = "StaffOnly")]
    public class ServicesController(
        DeactiveServiceUseCase _deactiveServiceUseCase,
        DeleteServiceUseCase _deleteServiceUseCase,
        GetAllServiceUseCase _getAllServiceUseCase,
        GetServiceByIdUseCase _getServiceByIdUseCase,
        CreateServiceUseCase _registerServiceUseCase
        ) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _getAllServiceUseCase.ExecuteAsync();
            return Response(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _getServiceByIdUseCase.ExecuteAsync(id);
            return Response(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateServiceRequest request)
        {
            var result = await _registerServiceUseCase.ExecuteAsync(request);
            if (!result.success)
            {
                return Conflict(new { message = result.error });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.data?.Id }, result.data);
        }

        [HttpPut("Deactive")]
        public async Task<IActionResult> Deactive([FromBody] DeactiveServiceRequest request)
        {
            var result = await _deactiveServiceUseCase.ExecuteAsync(request);
            return Response(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _deleteServiceUseCase.ExecuteAsync(id);
            return Response(result);
        }
    }
}
