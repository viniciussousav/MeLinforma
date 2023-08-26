namespace Domain.Shared;

public class EmptyResult
{
    public static readonly EmptyResult Empty = new ();
    private EmptyResult() { }
}