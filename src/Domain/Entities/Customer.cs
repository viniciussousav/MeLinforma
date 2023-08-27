namespace Domain.Entities;

public class Customer
{
    public static readonly Customer Empty = new();
    
    protected Customer() { }

    public Customer(string email)
    {
        Id = Guid.NewGuid();
        Email = email;
        Subscribed = true;
    }

    public Guid Id { get; }
    public string Email { get; } = string.Empty;
    public bool Subscribed { get; private set; }
    
    public void Unsubscribe()
    {
        Subscribed = false;
    }
    
    public void Subscribe()
    {
        Subscribed = true;
    }
}