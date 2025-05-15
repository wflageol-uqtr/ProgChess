using System.Security.Claims;
using ProChess.Server.Entities;

namespace ProgChess.Server.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    
    string GenerateRefreshToken();
    
    Task<string> SaveRefreshJwtTokenAsync(User user);

    Task<User?> ValidateRefreshToken(int userId, string refreshToken);
}