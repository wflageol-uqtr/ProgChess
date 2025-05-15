using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProgChess.Server.Database;
using ProChess.Server.Entities;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class AuthService(AppDbContext context, IConfiguration configuration, ITokenService tokenService) : IAuthService
{
    public async Task<TokenDto?> LoginAsync(UserDto request)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
        {
            return null;
        }

        //TODO: verifier les hashs
        if (user.Password != request.Password)
        {
            return null;
        }

        return await CreateTokenDto(user);
    }

    public async Task<TokenDto?> RefreshTokenAsync(RefreshTokenDto request)
    {
        var user = await tokenService.ValidateRefreshToken(request.Id, request.RefreshToken);
        if (user == null)
        {
            return null;
        }

        return await CreateTokenDto(user);
    }

    private async Task<TokenDto> CreateTokenDto(User user)
    {
        return new TokenDto
        {
            AccessToken = tokenService.GenerateAccessToken(user),
            RefreshToken = await tokenService.SaveRefreshJwtTokenAsync(user),
        };
    }
}    

 