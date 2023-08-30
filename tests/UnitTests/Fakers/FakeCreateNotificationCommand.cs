using Application.UseCases.CreateNotification;
using Bogus;
using Domain.Enums;

namespace UnitTests.Fakers;

public static class FakeCreateNotificationCommand
{
    public static CreateNotificationCommand CreateValid(DateTimeOffset? dateTimeOffset = null)
    {
        var faker = new Faker();
        return new CreateNotificationCommand(
            faker.Random.Guid(),
            faker.Random.Guid(),
            faker.Random.Word(),
            faker.Random.Words(),
            dateTimeOffset ?? DateTimeOffset.UtcNow,
            NotificationType.Web);
    }
    
    public static CreateNotificationCommand CreateInvalid()
    {
        var faker = new Faker();
        return new CreateNotificationCommand(
            faker.Random.Guid(),
            faker.Random.Guid(),
            string.Empty,
            string.Empty,
            DateTimeOffset.UtcNow,
            NotificationType.Undefined);
    }
}