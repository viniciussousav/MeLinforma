using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using WebApi.Customers.Models;

namespace WebApi.Customers;

[ApiController]
[Route("api/v1/customers")]
public class CustomersController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromServices] ICustomerRepository customerRepository)
    {
        try
        {
            var customers = await customerRepository.GetAll();
            
            return Ok(customers);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, [FromServices] ICustomerRepository customerRepository)
    {
        try
        {
            var customer = await customerRepository.Get(id);
        
            if (customer == Customer.Empty)
                return NotFound(new{ Error = $"Customer With Id {id} not found"});
            
            return Ok(customer);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(CreateCustomerRequest request, [FromServices] ICustomerRepository customerRepository)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(new{ Error = "Email can not be null"});

            var customerWithSameEmail = await customerRepository.Get(request.Email);
        
            if (customerWithSameEmail != Customer.Empty)
                return Conflict(new{ Error = $"Email {request.Email} already exists"});

            var customer = new Customer(request.Email);
            await customerRepository.Create(customer);

            return Ok(customer);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
    
    [HttpGet("{id:guid}/notifications")]
    public async Task<IActionResult> Notifications(
        Guid id, 
        [FromServices] ICustomerRepository customerRepository, 
        [FromServices] INotificationRepository notificationRepository)
    {
        try
        {
            var customer = await customerRepository.Get(id);

            if (customer == Customer.Empty)
                return NotFound(new {Error = $"Customer {id} not found"});

            var notifications = await notificationRepository.GetByCustomerId(id);
            
            return Ok(notifications);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
    
    [HttpPut("{id:guid}/notifications/subscribe")]
    public async Task<IActionResult> Subscribe(Guid id, [FromServices] ICustomerRepository customerRepository)
    {
        try
        {
            var customer = await customerRepository.Get(id);

            if (customer == Customer.Empty)
                return NotFound(new {Error = $"Customer {id} not found"});
            
            customer.Subscribe();
            await customerRepository.Update(customer);

            return Ok(customer);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
    
    [HttpPut("{id:guid}/notifications/unsubscribe")]
    public async Task<IActionResult> Unsubscribe(Guid id, [FromServices] ICustomerRepository customerRepository)
    {
        try
        {
            var customer = await customerRepository.Get(id);

            if (customer == Customer.Empty)
                return NotFound(new {Error = $"Customer {id} not found"});
            
            customer.Unsubscribe();
            await customerRepository.Update(customer);
            
            return Ok(customer);
        }
        catch (Exception e)
        {
            return Problem(title: e.Message, detail: e.StackTrace);
        }
    }
}