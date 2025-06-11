using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProgChess.Server.Database;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class AuthService(AppDbContext context, ITokenService tokenService, SignInManager<User> signInManager, ICookieService cookieService) : IAuthService
{
    public async Task<TokenDto> LoginAsync(UserDto request)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if (user == null)
                throw new BadRequestException("Courriel ou mot de passe invalide");
            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                throw new BadRequestException("Courriel ou mot de passe invalide");
            return await CreateTokenDto(user);
        }
        catch (BadRequestException e)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<TokenDto?> RefreshTokenAsync(RefreshTokenDto request)
    {
        try
        {
            var user = await tokenService.ValidateRefreshToken(request.UserId, request.RefreshToken);
            if (user == null)
                throw new UnauthorizedException("Refresh token est invalide");
            return await CreateTokenDto(user);
        }
        catch (UnauthorizedException e)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task LoginCodeAsync(StudentCodeDto request, HttpResponse response)
    {
        try
        {
            var exercise = await context.Exercises.FirstOrDefaultAsync(e => e.Id == request.ExerciseId);
            if (exercise == null)
                throw new BadRequestException("Code est invalide");
            if (!exercise.StudentCodes.Contains(request.Code))
                throw new BadRequestException("Code est invalide");
            cookieService.generateNormalCookie(response, request);
        }
        catch (BadRequestException e)
        {
            throw;
        }
        catch (Exception e)
        {
            
            throw new Exception(e.Message);
        }
    }

    public async Task VerifyCodeAsync(StudentCodeDto request)
    {
        try
        {
            var exercise = await context.Exercises.FirstOrDefaultAsync(e => e.Id == request.ExerciseId);
            if (exercise == null)
                throw new NotFoundException("Exercice introuvable");
            if (!exercise.StudentCodes.Contains(request.Code))
                throw new ForbiddenException("Vous ne pouvez pas accéder à cette exercice");
        }
        catch (NotFoundException e)
        {
            throw;
        }
        catch (ForbiddenException e)
        {
            throw;
        }
        catch (Exception e)
        {
            
            throw new Exception(e.Message);
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

 