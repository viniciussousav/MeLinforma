using Domain.Enums;
using Domain.Shared;

namespace Domain.Entities;

public class Notification
{
    public static readonly Notification Empty = new();

    protected Notification() { }
     
    public Notification(Guid id, Guid customerId, string title, string description, DateTimeOffset sendAt, NotificationType type)
    {
        Id = id;
        CustomerId = customerId;
        Title = title;
        Description = description;
        SendAt = sendAt;
        Type = type;
        Status = NotificationStatus.Pending;
    }

    public Guid Id { get; }
    public Guid CustomerId { get; }
    public string Title { get; } = string.Empty;
    public string Description { get; } = string.Empty;
    public DateTimeOffset SendAt { get; }
    public NotificationType Type { get; }
    public NotificationStatus Status { get; private set; }

    public void Sent()
    {
        if (Status != NotificationStatus.Pending)
            throw new DomainException(ErrorMessages.NotificationNotPending(Id));

        Status = NotificationStatus.Sent;
    }
    
    public void Confirm()
    {
        if (Status != NotificationStatus.Sent)
            throw new DomainException(ErrorMessages.NotificationNotSent(Id));
        
        Status = NotificationStatus.Succeeded;
    }
    
    public void Fail()
    {
        if (Status == NotificationStatus.Succeeded)
            throw new DomainException(ErrorMessages.NotificationAlreadyConfirmed(Id));
        
        Status = NotificationStatus.Failed;
    }
}