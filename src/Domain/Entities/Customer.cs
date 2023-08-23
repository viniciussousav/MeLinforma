namespace Domain.Entities;

public class Customer
{
    public static Customer Empty = new();
    
    private Customer() { }

    public Customer(string email)
    {
        Id = Guid.NewGuid();
        Email = email;
        Notify = true;
    }

    public Guid Id { get; }
    public string Email { get; } = string.Empty;
    public bool Notify { get; private set; }
    
    public void DisableNotifications()
    {
        Notify = false;
    }
    
    public void EnableNotifications()
    {
        Notify = true;
    }
}