using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application;

namespace TC1.RepairShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected new IActionResult Response<T>(BaseResponse<T> response)
        {
            if (response.success)
            {
                return Ok(response.data);
            }

            if (int.TryParse(response.StatusCode, out var code))
            {
                return code switch
                {
                    400 => BadRequest(response.error),
                    401 => Unauthorized(),
                    403 => Forbid(),
                    404 => NotFound(response.error),
                    500 => StatusCode(500, response.error),
                    _ => StatusCode(code, response.error),
                };
            }

            // fallback
            return BadRequest(response.error);
        }
    }
}
