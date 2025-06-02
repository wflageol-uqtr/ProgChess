using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Entities;
using ProgChess.Server.Dto;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ProgChess.Server.Services;

public interface IAuthService
{
    Task<TokenDto?> LoginAsync(UserDto request);
    Task<TokenDto?> RefreshTokenAsync(RefreshTokenDto request);
    Task<bool> LoginCodeAsync(StudentCodeDto request);
}