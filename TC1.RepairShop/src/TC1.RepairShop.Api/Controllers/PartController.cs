using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application.Parts.UseCases.DeactivePart;
using TC1.RepairShop.Application.Parts.UseCases.DeletePart;
using TC1.RepairShop.Application.Parts.UseCases.GetAllPart;
using TC1.RepairShop.Application.Parts.UseCases.GetPartById;
using TC1.RepairShop.Application.Parts.UseCases.ReceiveStock;
using TC1.RepairShop.Application.Parts.UseCases.RegisterPart;

namespace TC1.RepairShop.Api.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class PartController(
        DeactivatePartUseCase deactivatePartUseCase,
        DeletePartUseCase deletePartUseCase,
        GetAllPartUseCase getAllPartUseCase,
        GetPartByIdCaseUse getPartByIdCaseUse,
        ReceiveStockUseCase receiveStockUseCase,
        RegisterPartUseCase registerPartUseCase
        ) : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await getAllPartUseCase.ExecuteAsync();
            return Response(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await getPartByIdCaseUse.ExecuteAsync(id);
            return Response(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterPartRequest request)
        {
            var result = await registerPartUseCase.ExecuteAsync(request);
            return Response(result);
        }

        [HttpPut("ReceiveStock")]
        public async Task<IActionResult> ReceiveStock([FromBody] ReceiveStockRequest request)
        {
            var result = await receiveStockUseCase.ExecuteAsync(request);
            return Response(result);
        }

        [HttpPut("Deactive")]
        public async Task<IActionResult> Deactive([FromBody] DeactivePartRequest request)
        {
            var result = await deactivatePartUseCase.ExecuteAsync(request);
            return Response(result);
        }

        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeletePartRequest request)
        {
            var result = await deletePartUseCase.ExecuteAsync(request);
            return Response(result);
        }
    }
}
