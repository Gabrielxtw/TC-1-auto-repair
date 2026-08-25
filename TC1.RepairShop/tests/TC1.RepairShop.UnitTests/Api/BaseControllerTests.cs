using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
            .Which.Value.Should().Be("ok");
    }

    [Fact]
    public void Response_ShouldReturnUnauthorized_WhenStatusCodeIs401()
    {
        var response = new BaseResponse<string>(data: "", success: false, error: "unauthorized", StatusCode: "401");

        var result = _controller.InvokeResponse(response);

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Theory]
    [InlineData("500")]
    [InlineData("999")]
    public void Response_ShouldReturnStatusCodeResult_ForUnmappedOrServerErrorStatusCodes(string statusCode)
    {
        var response = new BaseResponse<string>(data: "", success: false, error: "boom", StatusCode: statusCode);

        var result = _controller.InvokeResponse(response);

        var objectResult = result.Should().BeOfType<ObjectResult>().Subject;
        objectResult.StatusCode.Should().Be(int.Parse(statusCode));
        objectResult.Value.Should().Be("boom");
    }
}
