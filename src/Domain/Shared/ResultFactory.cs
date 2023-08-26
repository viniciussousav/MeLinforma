namespace Domain.Shared;

public partial class Result
{
    public static Result<TValue> Fail<TValue>(params Error[] errors) => new(errors);
    public static Result<TValue> Success<TValue>(TValue data) => new(data);
    public static Result<TValue> Skip<TValue>() => new(true);

}