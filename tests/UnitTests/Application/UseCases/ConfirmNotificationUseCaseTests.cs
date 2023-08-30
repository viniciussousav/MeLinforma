using Application.UseCases.ConfirmNotification;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UnitTests.Fakers;

namespace UnitTests.Application.UseCases;

public class ConfirmNotificationUseCaseTests
{
    private readonly ILogger<ConfirmNotificationUseCase> _logger;
    private readonly INotificationRepository _notificationRepository;

    public ConfirmNotificationUseCaseTests()
    {
        _logger = Substitute.For<ILogger<ConfirmNotificationUseCase>>();
        _notificationRepository = Substitute.For<INotificationRepository>();
    }

    [Fact]
    public async Task Given_A_Command_When_Notification_Does_Not_Exists_Then_Nothing_Should_Be_Updated()
    {
        // Arrange
        var command = new ConfirmNotificationCommand(Guid.NewGuid());
        _notificationRepository.Get(command.NotificationId).Returns(Notification.Empty);

        var sut = new ConfirmNotificationUseCase(_logger, _notificationRepository);
        
        // Act
        await sut.Execute(command);
        
        // Assert
        await _notificationRepository.DidNotReceive().Update(Arg.Any<Notification>());
    }
    
    [Fact]
    public async Task Given_A_Command_When_Notification_Is_Already_Confirmed_Then_Nothing_Should_Be_Updated()
    {
        // Arrange
        var notification = FakeNotification.Create();
        notification.Sent();
        notification.Confirm();
        
        var command = new ConfirmNotificationCommand(notification.Id);

        _notificationRepository.Get(command.NotificationId).Returns(notification);

        var sut = new ConfirmNotificationUseCase(_logger, _notificationRepository);
        
        // Act
        await sut.Execute(command);
        
        // Assert
        await _notificationRepository.DidNotReceive().Update(Arg.Any<Notification>());
    }
    
    [Fact]
    public async Task Given_A_Command_When_Notification_Is_Not_Confirmed_Then_It_Should_Be_Updated()
    {
        // Arrange
        var notification = FakeNotification.Create();
        notification.Sent();
        
        var command = new ConfirmNotificationCommand(notification.Id);

        _notificationRepository.Get(command.NotificationId).Returns(notification);

        var sut = new ConfirmNotificationUseCase(_logger, _notificationRepository);
        
        // Act
        await sut.Execute(command);
        
        // Assert
        await _notificationRepository.Received(1).Update(Arg.Any<Notification>());
    }
}