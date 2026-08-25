using System;
using FluentAssertions;
using TC1.RepairShop.Domain.Enums;
using Xunit;

namespace TC1.RepairShop.UnitTests.Enums;

public class EnumerationTests
{
    [Fact]
    public void FromValue_ShouldReturnMatchingInstance()
    {
        var status = ServiceOrderStatus.FromValue(1);

        status.Should().Be(ServiceOrderStatus.Received);
    }

    [Fact]
    public void FromName_ShouldReturnMatchingInstance_CaseInsensitive()
    {
        var status = ServiceOrderStatus.FromName("delivered");

        status.Should().Be(ServiceOrderStatus.Delivered);
    }

    [Fact]
    public void GetAll_ShouldReturnEveryDeclaredValue()
    {
        var all = ServiceOrderStatus.GetAll();

        all.Should().HaveCount(7);
        all.Should().Contain(ServiceOrderStatus.Received);
        all.Should().Contain(ServiceOrderStatus.Cancelled);
    }

    [Fact]
    public void Equals_ShouldBeTrue_ForSameValue()
    {
        var a = ServiceOrderStatus.FromValue(1);
        var b = ServiceOrderStatus.FromValue(1);

        a.Equals(b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void FromValue_ShouldThrow_WhenValueDoesNotExist()
    {
        var act = () => ServiceOrderStatus.FromValue(999);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void FromName_ShouldThrow_WhenNameDoesNotExist()
    {
        var act = () => ServiceOrderStatus.FromName("NotAStatus");

        act.Should().Throw<InvalidOperationException>();
    }
}
