using Microsoft.AspNetCore.Mvc;
using System.Net;
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

                return response.StatusCode switch
                {
                    HttpStatusCode.OK => Ok(body),
                    HttpStatusCode.NoContent => NoContent(),
                    HttpStatusCode.BadRequest => BadRequest(body),
                    HttpStatusCode.Unauthorized => Unauthorized(),
                    HttpStatusCode.Forbidden => Forbid(),
                    HttpStatusCode.NotFound => NotFound(body),
                    _ => StatusCode((int)response.StatusCode, body),
                };
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
