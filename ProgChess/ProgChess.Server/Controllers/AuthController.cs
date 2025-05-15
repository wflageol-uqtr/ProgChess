using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProgChess.Server.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;;
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(UserDto request)
    {
        var result = await _authService.LoginAsync(request);
        if (result is null)
        {
            return BadRequest("Courriel ou mot de passe est invalide");
        }
        setTokenInsideCookie(result);
        Console.WriteLine("iciiiiii");
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshToken(RefreshTokenDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        if (result == null)
        {
            return Unauthorized("Refresh token invalid");
        }

        setTokenInsideCookie(result);
        return Ok(result);
    }
    
    [HttpGet("verify-token")]
    [Authorize]
    public IActionResult VerifyToken()
    {
        Console.WriteLine(HttpContext.Request.Cookies.Count);
        foreach (var cookie in  HttpContext.Request.Cookies)
        {
            Console.WriteLine(cookie);
        }
        
        return Ok();
    }

    private void setTokenInsideCookie(TokenDto token)
    {
        HttpContext.Response.Cookies.Append("accessToken", token.AccessToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(30),
            HttpOnly = true,
            IsEssential = true,
            Secure = false,
            SameSite = SameSiteMode.Lax
        });
        
        HttpContext.Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            Secure = false,
            SameSite = SameSiteMode.Lax
        });
    }
}