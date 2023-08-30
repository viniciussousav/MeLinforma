using Application.UseCases.CreateNotification;
using Application.Validation;
using Domain.Entities;
using Domain.Repositories;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UnitTests.Fakers;

namespace UnitTests.Application.UseCases;

public class CreateNotificationUseCaseTests
{
    private readonly ILogger<CreateNotificationUseCase> _logger;
    private readonly INotificationRepository _notificationRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<CreateNotificationCommand> _validator;

    public CreateNotificationUseCaseTests()
    {
        _logger = Substitute.For<ILogger<CreateNotificationUseCase>>();
        _notificationRepository = Substitute.For<INotificationRepository>();
        _customerRepository = Substitute.For<ICustomerRepository>();
        _validator = new CreateNotificationCommandValidator();
    }
    
    [Fact]
    public async Task Given_A_New_Command_When_Notification_Already_Exists_Then_Result_Should_Be_Success()
    {
        // Arrange
        var command = FakeCreateNotificationCommand.CreateValid();
        _notificationRepository.Get(command.NotificationId).Returns(FakeNotification.Create());

        var sut = new CreateNotificationUseCase(_logger, _notificationRepository, _customerRepository, _validator);

        // Act
        var result = await sut.Execute(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Given_A_New_Command_When_Notification_Is_Not_Valid_Then_Result_Should_Be_Fail()
    {
        // Arrange
        var command = FakeCreateNotificationCommand.CreateInvalid();
        
        _notificationRepository.Get(command.NotificationId).Returns(Notification.Empty);
        
        var sut = new CreateNotificationUseCase(_logger, _notificationRepository, _customerRepository, _validator);

        // Act
        var result = await sut.Execute(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Value.Should().BeNull();
    }
    
    [Fact]
    public async Task Given_A_New_Command_When_Customer_Not_Exists_Then_Result_Should_Be_Fail()
    {
        // Arrange
        var command = FakeCreateNotificationCommand.CreateValid();
        
        _notificationRepository.Get(command.NotificationId).Returns(Notification.Empty);
        _customerRepository.Get(command.CustomerId).Returns(Customer.Empty);

        var sut = new CreateNotificationUseCase(_logger, _notificationRepository, _customerRepository, _validator);

        // Act
        var result = await sut.Execute(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Value.Should().BeNull();
    }
    
    [Fact]
    public async Task Given_A_New_Command_When_Customer_Is_Not_Subscribed_Then_Result_Should_Be_Fail()
    {
        // Arrange
        var command = FakeCreateNotificationCommand.CreateValid();
        
        _notificationRepository.Get(command.NotificationId).Returns(Notification.Empty);

        var customer = FakeCustomer.Create();
        customer.Unsubscribe();
        
        _customerRepository.Get(command.CustomerId).Returns(customer);

        var sut = new CreateNotificationUseCase(_logger, _notificationRepository, _customerRepository, _validator);

        // Act
        var result = await sut.Execute(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Value.Should().BeNull();
    }
    
    [Fact]
    public async Task Given_A_New_Command_When_All_Requirements_Succeeds_Then_Result_Should_Be_Success()
    {
        // Arrange
        var command = FakeCreateNotificationCommand.CreateValid();
        
        _notificationRepository.Get(command.NotificationId).Returns(Notification.Empty);
        _customerRepository.Get(command.CustomerId).Returns(FakeCustomer.Create());

        var sut = new CreateNotificationUseCase(_logger, _notificationRepository, _customerRepository, _validator);

        // Act
        var result = await sut.Execute(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        result.Value.Should().NotBeNull();
    }
}