using Domain.Entities;

namespace Domain.Repositories;

public interface INotificationRepository
{
    Task<Notification> Get(Guid id);
    Task<IEnumerable<Notification>> GetByCustomerId(Guid customerId);
    
    Task Create(Notification notification);

    Task Update(Notification notification);
}