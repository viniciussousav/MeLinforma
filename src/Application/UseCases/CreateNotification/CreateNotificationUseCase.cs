using Application.Mapping;
using Domain.Entities;
using Domain.Repositories;
using Domain.Shared;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.CreateNotification;

public class CreateNotificationUseCase : ICreateNotificationUseCase
{
    private readonly ILogger<CreateNotificationUseCase> _logger;
    private readonly INotificationRepository _notificationRepository;
    private readonly IValidator<CreateNotificationCommand> _validator;

    public CreateNotificationUseCase(
        ILogger<CreateNotificationUseCase> logger, 
        INotificationRepository notificationRepository, 
        IValidator<CreateNotificationCommand> validator)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _validator = validator;
    }

    public async Task<Result<NotificationCreatedEvent>> Execute(CreateNotificationCommand command)
    {
        try
        {
            var notification = await _notificationRepository.Get(command.NotificationId);

            if (notification != Notification.Empty)
            {
                _logger.LogWarning("Notification {NotificationId} is already processed", command.NotificationId);
                return Result.Success(notification.MapToNotificationCreatedEvent());
            }
            
            await _notificationRepository.Create(notification);
            var validationResult = await _validator.ValidateAsync(command);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Notification {NotificationId} is not valid", command.NotificationId);
                var errors = validationResult.Errors.Select(failure => new Error(failure.PropertyName, failure.ErrorMessage)).ToArray();
                return Result.Fail<NotificationCreatedEvent>(errors);
            }
            
            _logger.LogError("Notification {NotificationId} successfully created", command.NotificationId);
            return Result.Success(notification.MapToNotificationCreatedEvent());
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An unexpected exception occured handling Notification {NotificationId}", command.NotificationId);
            throw;
        }
    }
}