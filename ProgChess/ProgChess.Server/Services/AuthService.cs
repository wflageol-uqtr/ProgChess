using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProgChess.Server.Database;
using ProChess.Server.Entities;
using ProgChess.Server.Dto;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ProgChess.Server.Services;

public class AuthService(AppDbContext context, ITokenService tokenService, SignInManager<User> signInManager) : IAuthService
{
    public async Task<TokenDto?> LoginAsync(UserDto request)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (user == null)
                return null;
            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return null;
            return await CreateTokenDto(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<TokenDto?> RefreshTokenAsync(RefreshTokenDto request)
    {
        var user = await tokenService.ValidateRefreshToken(request.UserId, request.RefreshToken);
        if (user == null)
        {
            return null;
        }

        return await CreateTokenDto(user);
    }

    public async Task<bool> LoginCodeAsync(StudentCodeDto request)
    {
        try
        {
            var exercise = await context.Exercises.FirstOrDefaultAsync(e => e.Id == request.ExerciceId);
            Console.WriteLine(request.ExerciceId);
            foreach (var code in exercise.StudentCodes)
            {
                Console.WriteLine(code);
            }
            return exercise.StudentCodes.Contains(request.Code);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    private async Task<TokenDto> CreateTokenDto(User user)
    {
        return new TokenDto
        {
            UserId = user.Id,
            AccessToken = tokenService.GenerateAccessToken(user),
            RefreshToken = await tokenService.SaveRefreshJwtTokenAsync(user),
        };
    }
    
}    

 