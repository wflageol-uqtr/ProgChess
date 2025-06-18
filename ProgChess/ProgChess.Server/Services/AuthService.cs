using System.Buffers.Text;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProgChess.Server.Database;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class AuthService(AppDbContext context, ITokenService tokenService, SignInManager<User> signInManager, ICookieService cookieService, UserManager<User> userManager, IEmailService emailService, IConfiguration configuration) : IAuthService
{
    public async Task<TokenDto> LoginAsync(UserDto request)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
            throw new BadRequestException("Courriel ou mot de passe invalide");
        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            throw new BadRequestException("Courriel ou mot de passe invalide 2");
        return await CreateTokenDto(user);
    }

    public async Task<TokenDto?> RefreshTokenAsync(RefreshTokenDto request)
    {
        var user = await tokenService.ValidateRefreshToken(request.UserId, request.RefreshToken);
        if (user == null)
            throw new UnauthorizedException("Refresh token est invalide");
        return await CreateTokenDto(user);
    }

    public async Task LoginCodeAsync(StudentCodeDto request, HttpResponse response)
    {
        bool exists = await context.StudentExercises
            .Where(se => se.ExerciseId == request.ExerciseId && se.Student.PermanentCode == request.Code)
            .AnyAsync();
        if (!exists)
            throw new BadRequestException("Code est invalide");
        cookieService.generateNormalCookie(response, request);
    }

    public async Task VerifyCodeAsync(StudentCodeDto request)
    {
        bool exists = await context.StudentExercises
            .Where(se => se.ExerciseId == request.ExerciseId && se.Student.PermanentCode == request.Code)
            .AnyAsync();
        if (!exists)
            throw new NotFoundException("Vérification du cookie est invalide");
    }

    public async Task ForgotPassword(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user != null)
        {
            // Serveur SMTP ?
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var link = $"{configuration["FrontendUrl"]}/admin/reset-password?email={user.Email}&activationToken={Base64UrlEncoder.Encode(token)}";
            Console.WriteLine(link);
            var response = await emailService.SendEmailAsync(user.Email!, "Reset Password", link);
            if (!response)
                throw new Exception("Erreur lors de l'envoie du courriel");
        }
    }

    public async Task ResetPassword(ResetPasswordDto request)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user == null)
            throw new NotFoundException("Email ou token invalide");
        var result = await userManager.ResetPasswordAsync(user, Base64UrlEncoder.Decode(request.Token), request.Password);
        result.Errors.ToList().ForEach(error => Console.WriteLine(error.Description));
        if (!result.Succeeded)
            throw new BadRequestException("Token invalide");
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

 