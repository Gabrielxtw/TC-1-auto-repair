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
            try
            {
                var body = new { data = response.data, success = response.success, error = response.error };

                if (int.TryParse(response.StatusCode, out var code))
                {
                    return code switch
                    {
                        200 => Ok(body),
                        204 => NoContent(),
                        400 => BadRequest(body),
                        401 => Unauthorized(),
                        403 => Forbid(),
                        404 => NotFound(body),
                        500 => StatusCode(500, body),
                        _ => StatusCode(code, body),
                    };
                }

                // fallback
                return BadRequest(body);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
