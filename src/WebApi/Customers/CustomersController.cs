using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.Customers.Models;

namespace WebApi.Customers;

[ApiController]
[Route("api/v1/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;

    public CustomersController(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(CreateCustomerRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new{ Error = "Email can not be null"});

            var customerWithSameEmail = await _customerRepository.Get(request.Email);
        
            if (customerWithSameEmail != Customer.Empty)
                return Conflict(new{ Error = $"Email {request.Email} already exists"});

            var customer = new Customer(request.Email);
            await _customerRepository.Create(customer);

            return Ok(customer);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            var customer = await _customerRepository.Get(id);
        
            if (customer == Customer.Empty)
                return NotFound(new{ Error = $"Customer With Id {id} not found"});
            
            return Ok(customer);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var customers = await _customerRepository.GetAll();
            
            return Ok(customers);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
}