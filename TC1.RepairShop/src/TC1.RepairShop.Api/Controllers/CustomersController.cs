using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TC1.RepairShop.Domain.Registration;

namespace TC1.RepairShop.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var customers = new List<Customer>();
        return Ok(customers);
    }
}
