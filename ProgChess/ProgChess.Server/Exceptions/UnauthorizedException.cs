namespace ProChess.Server.Exceptions;

public class UnauthorizedException: Exception
{
    public UnauthorizedException(string? message) : base(message)
    {
    }
}