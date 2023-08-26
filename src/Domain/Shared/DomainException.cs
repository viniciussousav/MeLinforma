namespace Domain.Shared;

public class DomainException : Exception
{
    public DomainException()
    {
        Error = new Error("DomainException", "A domain validation occured");
    }

    public DomainException(string message)
        : base(message)
    {
        Error = new Error("DomainException", message);
    }

    public DomainException(string message, Exception inner)
        : base(message, inner)
    {
        Error = new Error("DomainException", message);
    }
    
    public DomainException(Error error)
        : base(error.Message)
    {
        Error = error;
    }
    
    public Error Error { get; init; }
    
    
}