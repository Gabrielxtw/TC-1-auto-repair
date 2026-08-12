using System;
using TC1.RepairShop.Domain.Registration;
using TC1.RepairShop.Domain.Common;
using Xunit;

namespace TC1.RepairShop.UnitTests.Registration;

public class CustomerTests
{
    [Fact]
    public void Create_ShouldInitializeCustomer()
    {
        var customer = Customer.Create("John Doe", "529.982.247-25", "(11) 99999-9999", "john@example.com");

        Assert.NotEqual(Guid.Empty, customer.Id);
        Assert.Equal("John Doe", customer.Name);
        Assert.Equal("52998224725", customer.NationalId);
        Assert.Equal("(11) 99999-9999", customer.Phone);
        Assert.Equal("john@example.com", customer.Email);
        Assert.Equal(Status.Active, customer.Status);
    }

    [Fact]
    public void UpdateContactInfo_ShouldChangePhoneAndEmail()
    {
        var customer = Customer.Create("John Doe", "529.982.247-25", "(11) 99999-9999", "john@example.com");

        customer.UpdateContactInfo("(22) 88888-8888", "john2@example.com");

        Assert.Equal("(22) 88888-8888", customer.Phone);
        Assert.Equal("john2@example.com", customer.Email);
    }

    [Fact]
    public void Delete_ShouldSetStatusDeleted()
    {
        var customer = Customer.Create("John Doe", "529.982.247-25", "(11) 99999-9999", "john@example.com");

        customer.Delete();

        Assert.Equal(Status.Deleted, customer.Status);
    }
}
