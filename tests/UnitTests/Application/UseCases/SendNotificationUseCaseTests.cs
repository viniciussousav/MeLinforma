using Application.Services;
using Application.UseCases.SendNotification;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UnitTests.Fakers;

namespace UnitTests.Application.UseCases;

public class SendNotificationUseCaseTests
{
    private readonly ILogger<SendNotificationUseCase> _logger;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IHubContext<NotificationsHub> _notificationsHub;

    public SendNotificationUseCaseTests()
    {
        _logger = Substitute.For<ILogger<SendNotificationUseCase> >();
        _notificationRepository = Substitute.For<INotificationRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _notificationsHub = Substitute.For<IHubContext<NotificationsHub>>();
    }

    [Fact]
    public async Task Given_A_Command_When_Notification_Does_Not_Exist_Then_Result_Should_Be_Fail()
    {
        // Arrange
        var command = new SendNotificationCommand(Guid.NewGuid());

        _notificationRepository.Get(command.NotificationId).Returns(Notification.Empty);

        var sut = new SendNotificationUseCase(_logger, _notificationsHub, _notificationRepository, _customerRepository);
        
        // Act
        var result = await sut.Execute(command);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Given_A_Command_When_Notification_Is_Already_Succeeded_Then_Result_Should_Be_Fail()
    {
        // Arrange
        var command = new SendNotificationCommand(Guid.NewGuid());

        var notification = FakeNotification.Create();
        notification.Sent();
        notification.Confirm();
        
        _notificationRepository.Get(command.NotificationId).Returns(notification);
        
        var sut = new SendNotificationUseCase(_logger, _notificationsHub, _notificationRepository, _customerRepository);
        
        // Act
        var result = await sut.Execute(command);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.ShouldSkip.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
    
    [Fact]
    public async Task Given_A_Command_When_Customer_Does_Not_Exists_Then_Result_Should_Be_Fail()
    {
        // Arrange
        var command = new SendNotificationCommand(Guid.NewGuid());

        var notification = FakeNotification.Create();
        
        _notificationRepository.Get(command.NotificationId).Returns(notification);
        _customerRepository.Get(notification.CustomerId).Returns(Customer.Empty);
        
        var sut = new SendNotificationUseCase(_logger, _notificationsHub, _notificationRepository, _customerRepository);
        
        // Act
        var result = await sut.Execute(command);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task Given_A_Command_When_Customer_Is_Not_Subscribed_Then_Result_Should_Be_Fail()
    {
        // Arrange
        var command = new SendNotificationCommand(Guid.NewGuid());

        var notification = FakeNotification.Create();
        _notificationRepository.Get(command.NotificationId).Returns(notification);

        var customer = FakeCustomer.Create();
        customer.Unsubscribe();
        _customerRepository.Get(notification.CustomerId).Returns(customer);
        
        var sut = new SendNotificationUseCase(_logger, _notificationsHub, _notificationRepository, _customerRepository);
        
        // Act
        var result = await sut.Execute(command);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Given_A_New_Command_When_All_Requirements_Succeeds_Then_Result_Should_Be_Success()
    {
        // Arrange
        var command = new SendNotificationCommand(Guid.NewGuid());

        var notification = FakeNotification.Create();
        _notificationRepository.Get(command.NotificationId).Returns(notification);

        var customer = FakeCustomer.Create();
        _customerRepository.Get(notification.CustomerId).Returns(customer);
        
        var sut = new SendNotificationUseCase(_logger, _notificationsHub, _notificationRepository, _customerRepository);
        
        // Act
        var result = await sut.Execute(command);
        
        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}