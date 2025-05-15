using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IAuthService
{
    Task<TokenDto?> LoginAsync(UserDto request);
    Task<TokenDto?> RefreshTokenAsync(RefreshTokenDto request);
}