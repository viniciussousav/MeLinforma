using Domain.Entities;

namespace Domain.Repositories;

public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetAll();
    
    Task<Customer> Get(Guid id);
    
    Task<Customer> Get(string email);
    
    Task Create(Customer customer);

    Task Update(Customer customer);
}