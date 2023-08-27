using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

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
        return await _dbContext.Notifications.FirstOrDefaultAsync(n => n.Id == id) ?? Notification.Empty;
    }
    
    public async Task<IEnumerable<Notification>> GetByCustomerId(Guid customerId)
    {
        return await _dbContext.Notifications
            .Where(n => n.CustomerId == customerId && (n.Status == NotificationStatus.Succeeded || n.Status == NotificationStatus.Succeeded))
            .ToListAsync();
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