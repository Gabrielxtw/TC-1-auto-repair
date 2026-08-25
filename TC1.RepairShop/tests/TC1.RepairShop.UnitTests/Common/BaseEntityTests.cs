using FluentAssertions;
using TC1.RepairShop.Domain.CustomExceptions;
using TC1.RepairShop.Domain.Entities.Parts;
using TC1.RepairShop.Domain.Enums;
using Xunit;

namespace TC1.RepairShop.UnitTests.Common;

public class BaseEntityTests
{
    [Fact]
    public void Activate_ShouldSetStatusActive()
    {
        var part = Part.Create("Brake Pad", 19.99m);
        part.Deactivate();

        part.Activate();

        part.Status.Should().Be(Status.Active);
    }

    [Fact]
    public void Deactivate_ShouldThrow_WhenEntityIsAlreadyInactive()
    {
        var part = Part.Create("Brake Pad", 19.99m);
        part.Deactivate();

        var act = () => part.Deactivate();

        act.Should().Throw<BusinessException>()
            .WithMessage("Cannot do action on an inactive entity.");
    }
}
