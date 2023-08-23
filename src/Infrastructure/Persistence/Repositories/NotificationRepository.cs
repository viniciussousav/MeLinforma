using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly MeLinformaDbContext _dbContext;

    public NotificationRepository(MeLinformaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Notification> Get(Guid id)
    {
        return await _dbContext.Notifications.FindAsync(new { Id = id }) ?? Notification.Empty;
    }
    
    public async Task Create(Notification notification)
    {
        await _dbContext.Notifications.AddAsync(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Notification notification)
    {
        _dbContext.Notifications.Update(notification);
        await _dbContext.SaveChangesAsync();
    }
}