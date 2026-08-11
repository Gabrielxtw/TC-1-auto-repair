using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TC1.RepairShop.Api.Controllers;

[ApiController]
[Authorize(Policy = "CustomerOnly")]
[Route("api/customers/me/quotes")]
public class MyQuotesController : ControllerBase
{
    [HttpGet]
    public IActionResult GetMyQuotes()
    {
        // TODO(ServiceOrders/Quotes bounded context): depende de IQuoteRepository/IServiceOrderRepository,
        // que ainda não existem. Implementar quando esses repositórios estiverem prontos.
        return Ok(Array.Empty<object>());
    }
}
