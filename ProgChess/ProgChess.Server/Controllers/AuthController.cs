using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProgChess.Server.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService, ICookieService _cookieService): ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(UserDto request)
    {
        var result = await _authService.LoginAsync(request);
        if (result is null)
        {
            return BadRequest("Courriel ou mot de passe est invalide");
        }
        _cookieService.generateHttpOnlyCookie(Response, result);
        return Ok(result);
    }

    [HttpPost("login-code")]
    public IActionResult LoginCode(StudentCodeDto request)
    {
        var validCodes = System.IO.File.ReadAllLines("Codes.txt");
        
        if (validCodes.Contains(request.Code))
        {
           _cookieService.generateNormalCookie(Response, request);
            return Ok();
        }
        
        return BadRequest("Code est invalide");
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshToken(RefreshTokenDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        if (result == null)
        {
            return Unauthorized("Refresh token invalid");
        }
        _cookieService.generateHttpOnlyCookie(Response, result);
        return Ok(result);
    }
    
    [HttpGet("verify-token")]
    [Authorize]
    public IActionResult VerifyToken()
    {
        return Ok();
    }
    
}