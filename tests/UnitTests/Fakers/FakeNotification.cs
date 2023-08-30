using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace UnitTests.Fakers;

public static class FakeNotification
{
    public static Notification Create(DateTimeOffset? dateTimeOffset = null)
    {
        var faker = new Faker();
        return new Notification(
            faker.Random.Guid(),
            faker.Random.Guid(),
            faker.Random.Word(),
            faker.Random.Words(),
            dateTimeOffset ?? DateTimeOffset.UtcNow,
            NotificationType.Web);
    }
}