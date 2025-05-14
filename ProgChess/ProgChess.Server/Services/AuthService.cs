using Microsoft.EntityFrameworkCore;
using ProChess.Server.Database;
using ProChess.Server.Entities;
using ProChess.Server.Models;

namespace ProChess.Server.Services;

public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
{
    public async Task<string?> LoginAsync(UserDto request)
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

        return CreateJwtToken(user);
    }

    private string CreateJwtToken(User user)
    {
        // implement token
        return "token";
    }
}