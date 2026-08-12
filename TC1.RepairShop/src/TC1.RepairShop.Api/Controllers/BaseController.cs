using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Application;

namespace TC1.RepairShop.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected new IActionResult Response<T>(BaseResponse<T> response)
        {
            if (response.success)
            {
                return Ok(response.data);
            }

            switch (response.StatusCode)
            {
                case "400":
                    return BadRequest(response.error);
                case "401":
                    return Unauthorized();
                case "500":
                    return BadRequest(response.error);
                default:
                    return BadRequest(response.error);
            }
        }
    }
}
