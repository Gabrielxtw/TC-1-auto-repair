using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Services.UseCases;

namespace TC1.RepairShop.Api.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Authorize(Policy = "StaffOnly")]
    public class ServicesController(
        DeactiveServiceUseCase deactiveServiceUseCase,
        DeleteServiceUseCase deleteServiceUseCase,
        GetAllServiceUseCase getAllServiceUseCase,
        GetServiceByIdUseCase getServiceByIdUseCase,
        CreateServiceUseCase registerServiceUseCase
        ) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await getAllServiceUseCase.ExecuteAsync();
            return Response(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await getServiceByIdUseCase.ExecuteAsync(id);
            return Response(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] CreateServiceRequest request)
        {
            var result = await registerServiceUseCase.ExecuteAsync(request);
            return Response(result);
        }

        [HttpPut("Deactive")]
        public async Task<IActionResult> Deactive([FromBody] DeactiveServiceRequest request)
        {
            var result = await deactiveServiceUseCase.ExecuteAsync(request);
            return Response(result);
        }

        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteServiceRequest request)
        {
            var result = await deleteServiceUseCase.ExecuteAsync(request);
            return Response(result);
        }
    }
}
