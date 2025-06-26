namespace ProChess.Server.Exceptions;

public class ExecutionErrorException: Exception
{
    public ExecutionErrorException(string? message) : base(message)
    {
    }
}