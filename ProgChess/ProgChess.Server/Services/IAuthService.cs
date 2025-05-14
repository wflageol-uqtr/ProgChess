using ProChess.Server.Models;

namespace ProChess.Server.Services;

public interface IAuthService
{
    Task<string?> LoginAsync(UserDto request);
}