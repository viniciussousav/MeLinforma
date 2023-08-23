using Domain.Entities;

namespace Domain.Repositories;

public interface INotificationRepository
{
    Task<Notification> Get(Guid id);
    
    Task Create(Notification notification);

    Task Update(Notification notification);
}