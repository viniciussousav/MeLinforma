namespace Domain.Shared;

public partial class Result
{
    protected Result() { }
    
    protected Result(params Error[] errors)
    {
        Errors.AddRange(errors);
    }
    
    public List<Error> Errors { get; } = new();
    
    public bool IsValid => !Errors.Any();

    public bool ShouldSkip { get; protected init; }
}

public class Result<T> : Result
{
    public Result(T value) : base()
    {
        Value = value;
    }
    
    public Result(bool skip)
    {
        ShouldSkip = skip;
    }
    
    public Result(params Error[] errors) : base(errors) { }

    public T? Value { get; }
}