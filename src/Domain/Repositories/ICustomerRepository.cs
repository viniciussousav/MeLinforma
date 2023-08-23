using Domain.Entities;

namespace Domain.Repositories;

public interface ICustomerRepository
{
    Task<Customer> Get(Guid id);
    
    Task Create(Customer customer);

    Task Update(Customer customer);
}