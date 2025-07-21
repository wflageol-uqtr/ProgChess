using System.Security.Claims;

namespace ProChess.Server.Context;

public interface IUserContext
{
    string? UserId { get; }
}

public class UserContext(IHttpContextAccessor httpContextAccessor): IUserContext
{
    public string UserId => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) 
                            ?? null;
}