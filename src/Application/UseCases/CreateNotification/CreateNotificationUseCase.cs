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
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<CreateNotificationCommand> _validator;

    public CreateNotificationUseCase(
        ILogger<CreateNotificationUseCase> logger, 
        INotificationRepository notificationRepository,
        ICustomerRepository customerRepository,
        IValidator<CreateNotificationCommand> validator)
    {
        _logger = logger;
        _notificationRepository = notificationRepository;
        _validator = validator;
        _customerRepository = customerRepository;
    }

    public async Task<Result<Notification>> Execute(CreateNotificationCommand command)
    {
        try
        {
            var notification = await _notificationRepository.Get(command.NotificationId);

            if (notification == Notification.Empty)
            {
                _logger.LogWarning("Notification {NotificationId} is already processed", command.NotificationId);
                return Result.Success(notification);
            }
            
            await _notificationRepository.Create(notification);
            var validationResult = await _validator.ValidateAsync(command);
            
            if (!validationResult.IsValid)
            {
                _logger.LogError("Notification {NotificationId} is not valid", command.NotificationId);
                var errors = validationResult.Errors.Select(failure => new Error(failure.PropertyName, failure.ErrorMessage)).ToArray();
                return Result.Fail<Notification>(errors);
            }

            var customer = await _customerRepository.Get(command.CustomerId);

            if (customer == Customer.Empty || !customer.Notify)
            {
                _logger.LogError("Customer {CustomerId} is not valid", command.CustomerId);
                var error = new Error("Customer", $"Customer {command.CustomerId} is not valid to be notified");
                return Result.Fail<Notification>(error);
            }

            await _notificationRepository.Create(notification);
            
            _logger.LogError("Notification {NotificationId} successfully created", command.NotificationId);
            return Result.Success(notification);
        }
        catch (Exception e)
        {
            _logger.LogError(e,"An unexpected exception occured handling Notification {NotificationId}", command.NotificationId);
            throw;
        }
    }
}