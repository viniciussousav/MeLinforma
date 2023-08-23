using Domain.Entities;
using Infrastructure.Persistence.Configuration.EntityType;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CS8618

namespace Infrastructure.Persistence.Contexts;

public class MeLinformaDbContext : DbContext
{
    public MeLinformaDbContext(DbContextOptions<MeLinformaDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new CustomerEntityTypeConfiguration().Configure(modelBuilder.Entity<Customer>());
        new NotificationEntityTypeConfiguration().Configure(modelBuilder.Entity<Notification>());
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Notification> Notifications { get; set; }

}