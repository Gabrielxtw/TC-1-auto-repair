using FluentAssertions;
using Moq;
using TC1.RepairShop.Application.Notifications;
using TC1.RepairShop.Application.Quotes.EventHandlers;
using TC1.RepairShop.Domain.Entities.ServiceOrders;
using TC1.RepairShop.Domain.Enums;
using TC1.RepairShop.Domain.Events;
using TC1.RepairShop.Domain.Interfaces;
using Xunit;

namespace TC1.RepairShop.UnitTests.EventHandlers;

public class QuoteApprovedEventHandlerTests
{
    private static ServiceOrder CreateAwaitingApprovalOrder()
    {
        var order = ServiceOrder.Create(Guid.NewGuid(), Guid.NewGuid());
        order.AdvanceTo(ServiceOrderStatus.UnderDiagnosis);
        order.AdvanceTo(ServiceOrderStatus.AwaitingApproval);
        return order;
    }

    [Fact]
    public async Task Handle_ShouldAdvanceOrderToInProgress_WhenOrderExists()
    {
        var order = CreateAwaitingApprovalOrder();
        var repository = new Mock<IServiceOrderRepository>();
        var mail = new Mock<IEmailSender>();
        var quoteRepository = new Mock<IQuoteRepository>();
        var partRepository = new Mock<IPartRepository>();
        var servicePartRepository = new Mock<IServiceOrderPartRepository>();

        // create a quote and attach it to the order
        var quote = TC1.RepairShop.Domain.Entities.Quotes.Quote.Create(order.Id, 200m);
        quoteRepository.Setup(r => r.GetByIdAsync(quote.Id)).ReturnsAsync(quote);
        // set private QuoteId on order
        typeof(ServiceOrder).GetProperty("QuoteId")!.SetValue(order, quote.Id);

        // create part and service order part (not supplied by customer)
        var part = TC1.RepairShop.Domain.Entities.Parts.Part.Create("Brake", 100m, stockQuantity: 10);
        var servicePart = TC1.RepairShop.Domain.Entities.ServiceOrders.ServiceOrderPart.Create(order.Id, part.Id, quantity: 2, price: part.Price, suppliedByCustomer: false);
        order.ServiceOrderParts.Add(servicePart);

        // setup repositories
        repository.Setup(r => r.GetByIdDetailedAsync(order.Id)).ReturnsAsync(order);
        servicePartRepository.Setup(r => r.GetByServiceOrderIdAsync(order.Id)).ReturnsAsync(new[] { servicePart });
        partRepository.Setup(r => r.GetByIdAsync(part.Id)).ReturnsAsync(part);

        var handler = new QuoteApprovedEventHandler(repository.Object, partRepository.Object, servicePartRepository.Object, quoteRepository.Object, mail.Object);
        var domainEvent = new QuoteApprovedEvent(order.Id);

        await handler.Handle(domainEvent);

        order.OrderStatusValue.Should().Be(ServiceOrderStatus.InProgress);
        repository.Verify(r => r.UpdateAsync(order), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldNotUpdate_WhenOrderDoesNotExist()
    {
        var repository = new Mock<IServiceOrderRepository>();
        var mail = new Mock<IEmailSender>();
        var quoteRepository = new Mock<IQuoteRepository>();
        var partRepository = new Mock<IPartRepository>();
        var servicePartRepository = new Mock<IServiceOrderPartRepository>();
        repository.Setup(r => r.GetByIdDetailedAsync(It.IsAny<Guid>())).ReturnsAsync((ServiceOrder?)null);

        var handler = new QuoteApprovedEventHandler(repository.Object, partRepository.Object, servicePartRepository.Object, quoteRepository.Object, mail.Object);
        var domainEvent = new QuoteApprovedEvent(Guid.NewGuid());

        await handler.Handle(domainEvent);

        repository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldSwallowException_WhenRepositoryThrowsOnGetById()
    {
        var mail = new Mock<IEmailSender>();
        var quoteRepository = new Mock<IQuoteRepository>();
        var partRepository = new Mock<IPartRepository>();
        var repository = new Mock<IServiceOrderRepository>();
        var servicePartRepository = new Mock<IServiceOrderPartRepository>();
        repository.Setup(r => r.GetByIdDetailedAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("db unavailable"));

        var handler = new QuoteApprovedEventHandler(repository.Object, partRepository.Object, servicePartRepository.Object, quoteRepository.Object, mail.Object);
        var domainEvent = new QuoteApprovedEvent(Guid.NewGuid());

        var act = async () => await handler.Handle(domainEvent);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Handle_ShouldSwallowException_WhenRepositoryThrowsOnUpdate()
    {
        var mail = new Mock<IEmailSender>();
        var quoteRepository = new Mock<IQuoteRepository>();
        var partRepository = new Mock<IPartRepository>();
        var order = CreateAwaitingApprovalOrder();
        var repository = new Mock<IServiceOrderRepository>();
        var servicePartRepository = new Mock<IServiceOrderPartRepository>();
        repository.Setup(r => r.GetByIdDetailedAsync(order.Id)).ReturnsAsync(order);
        repository.Setup(r => r.UpdateAsync(It.IsAny<ServiceOrder>())).ThrowsAsync(new Exception("db unavailable"));

        var handler = new QuoteApprovedEventHandler(repository.Object, partRepository.Object, servicePartRepository.Object, quoteRepository.Object, mail.Object);
        var domainEvent = new QuoteApprovedEvent(order.Id);

        var act = async () => await handler.Handle(domainEvent);

        await act.Should().NotThrowAsync();
    }
}
