using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProChess.Server.Entities;
using ProgChess.Server.Database;

namespace ProgChess.Server.Services;

public class TokenService(IConfiguration configuration, AppDbContext context) : ITokenService
{
    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtSettings:Secret")!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("JwtSettings:Issuer"),
            audience: configuration.GetValue<string>("JwtSettings:Audience"),
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
            ],
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }
    
    public async Task<string> SaveRefreshJwtTokenAsync(User user)
    {
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        await context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<User?> ValidateRefreshToken(string userId, string refreshToken)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
        {
            return null;
        }
        return user;
    }
}