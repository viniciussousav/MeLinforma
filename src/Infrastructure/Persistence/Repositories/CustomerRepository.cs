using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly MeLinformaDbContext _dbContext;

    public CustomerRepository(MeLinformaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Customer>> GetAll()
    {
        return await _dbContext.Customers.ToListAsync();
    }
    
    public async Task<Customer> Get(Guid id)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id) ?? Customer.Empty;
    }
    
    public async Task<Customer> Get(string email)
    {
        return await _dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email) ?? Customer.Empty;
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