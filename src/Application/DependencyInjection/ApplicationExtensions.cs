using Application.UseCases.CreateNotification;
using Application.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddValidators();
        services.AddUseCases();
        
        return services;
    }
    
    private static void AddValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<CreateNotificationCommand>, CreateNotificationCommandValidator>();
    }
    
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ICreateNotificationUseCase, CreateNotificationUseCase>();
    }
}