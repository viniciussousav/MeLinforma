namespace WebApi.Customers.Models;

public record CreateCustomerRequest
{
    public string Email { get; init; } = string.Empty;
}