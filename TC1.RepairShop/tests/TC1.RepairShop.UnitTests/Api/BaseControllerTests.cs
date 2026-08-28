using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TC1.RepairShop.Api.Controllers;
using TC1.RepairShop.Application;
using Xunit;

namespace TC1.RepairShop.UnitTests.Api;

public class TestableController : BaseController
{
    public IActionResult InvokeResponse<T>(BaseResponse<T> response) => Response(response);
}

public class BaseControllerTests
{
    private readonly TestableController _controller = new();

    [Fact]
    public void Response_ShouldReturnOk_WhenSuccessIsTrue()
    {
        var response = new BaseResponse<string>(data: "ok", success: true);

        var result = _controller.InvokeResponse(response);

        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new { data = "ok", error = "", success = true });
    }

    [Fact]
    public void Response_ShouldReturnUnauthorized_WhenStatusCodeIs401()
    {
        var response = new BaseResponse<string>(data: "", success: false, error: "unauthorized", StatusCode: HttpStatusCode.Unauthorized);

        var result = _controller.InvokeResponse(response);

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    public void Response_ShouldReturnStatusCodeResult_ForUnmappedOrServerErrorStatusCodes(HttpStatusCode statusCode)
    {
        var response = new BaseResponse<string>(data: "", success: false, error: "boom", StatusCode: statusCode);

        var result = _controller.InvokeResponse(response);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(((int)statusCode));
        objectResult.Value.Should().BeEquivalentTo(new { data = "", error = "boom", success = false });
    }
}
