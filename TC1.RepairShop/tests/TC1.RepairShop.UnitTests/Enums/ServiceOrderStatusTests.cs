using FluentAssertions;
using TC1.RepairShop.Domain.Enums;
using Xunit;

namespace TC1.RepairShop.UnitTests.Enums;

public class ServiceOrderStatusTests
{
    [Theory]
    [InlineData("Received", "Under Diagnosis")]
    [InlineData("Received", "Cancelled")]
    [InlineData("Under Diagnosis", "Awaiting Approval")]
    [InlineData("Under Diagnosis", "Cancelled")]
    [InlineData("Awaiting Approval", "In Progress")]
    [InlineData("Awaiting Approval", "Under Diagnosis")]
    [InlineData("In Progress", "Completed")]
    [InlineData("Completed", "Delivered")]
    public void CanTransitionTo_ShouldReturnTrue_ForValidTransitions(string from, string to)
    {
        var current = ServiceOrderStatus.FromName(from);
        var next = ServiceOrderStatus.FromName(to);

        current.CanTransitionTo(next).Should().BeTrue();
    }

    [Theory]
    [InlineData("Received", "Delivered")]
    [InlineData("Received", "In Progress")]
    [InlineData("Under Diagnosis", "In Progress")]
    [InlineData("In Progress", "Delivered")]
    [InlineData("Delivered", "Received")]
    [InlineData("Delivered", "Cancelled")]
    [InlineData("Cancelled", "Received")]
    [InlineData("Cancelled", "Under Diagnosis")]
    public void CanTransitionTo_ShouldReturnFalse_ForInvalidTransitions(string from, string to)
    {
        var current = ServiceOrderStatus.FromName(from);
        var next = ServiceOrderStatus.FromName(to);

        current.CanTransitionTo(next).Should().BeFalse();
    }
}
