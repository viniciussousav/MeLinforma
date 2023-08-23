using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly MeLinformaDbContext _dbContext;

    public CustomerRepository(MeLinformaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer> Get(Guid id)
    {
        return await _dbContext.Customers.FindAsync(new { Id = id }) ?? Customer.Empty;
    }

    public async Task Create(Customer customer)
    {
        await _dbContext.Customers.AddAsync(customer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Customer customer)
    {
        _dbContext.Customers.Update(customer);
        await _dbContext.SaveChangesAsync();
    }
}