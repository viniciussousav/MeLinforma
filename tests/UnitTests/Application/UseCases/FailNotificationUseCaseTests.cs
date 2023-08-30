using Application.UseCases.ConfirmNotification;
using Application.UseCases.FailNotification;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UnitTests.Fakers;

namespace UnitTests.Application.UseCases;

public class FailNotificationUseCaseTests
{
    private readonly ILogger<FailNotificationUseCase> _logger;
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IEnumerable<Error> _errors;

    public FailNotificationUseCaseTests()
    {
        _logger = Substitute.For<ILogger<FailNotificationUseCase>>();
        _notificationRepository = Substitute.For<INotificationRepository>();
        _errors = new List<Error>() {new Error("Test", "Mock")};
    }

    [Fact]
    public async Task Given_A_Command_When_Notification_Does_Not_Exists_Then_Nothing_Should_Be_Updated()
    {
        // Arrange
        var command = new FailNotificationCommand { NotificationId = Guid.NewGuid(), Errors = _errors };
        _notificationRepository.Get(command.NotificationId).Returns(Notification.Empty);

        var sut = new FailNotificationUseCase(_logger, _notificationRepository);
        
        // Act
        await sut.Execute(command);
        
        // Assert
        await _notificationRepository.DidNotReceive().Update(Arg.Any<Notification>());
    }
    
    [Fact]
    public async Task Given_A_Command_When_Notification_Is_Already_Failed_Then_Nothing_Should_Be_Updated()
    {
        // Arrange
        var notification = FakeNotification.Create();
        notification.Sent();
        notification.Fail();
        
        var command = new FailNotificationCommand { NotificationId = notification.Id, Errors = _errors };

        _notificationRepository.Get(command.NotificationId).Returns(notification);

        var sut = new FailNotificationUseCase(_logger, _notificationRepository);
        
        // Act
        await sut.Execute(command);
        
        // Assert
        await _notificationRepository.DidNotReceive().Update(Arg.Any<Notification>());
    }
    
    [Fact]
    public async Task Given_A_Command_When_Notification_Is_Not_Failed_Then_It_Should_Be_Updated()
    {
        // Arrange
        var notification = FakeNotification.Create();
        notification.Sent();
        
        var command = new FailNotificationCommand { NotificationId = notification.Id, Errors = _errors };

        _notificationRepository.Get(command.NotificationId).Returns(notification);

        var sut = new FailNotificationUseCase(_logger, _notificationRepository);
        
        // Act
        await sut.Execute(command);
        
        // Assert
        await _notificationRepository.Received(1).Update(Arg.Any<Notification>());
    }
}