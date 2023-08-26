using Application.UseCases.ConfirmNotification;
using Application.UseCases.CreateNotification;
using Application.UseCases.FailNotification;
using Application.UseCases.SendNotification;
using Application.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class ApplicationExtensions
{
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddValidators();
        services.AddUseCases();
    }
    
    private static void AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<CreateNotificationCommand>, CreateNotificationCommandValidator>();
    }
    
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateNotificationUseCase, CreateNotificationUseCase>();
        services.AddScoped<ISendNotificationUseCase, SendNotificationUseCase>();
        services.AddScoped<IConfirmNotificationUseCase, ConfirmNotificationUseCase>();
        services.AddScoped<IFailNotificationUseCase, FailNotificationUseCase>();
    }
}