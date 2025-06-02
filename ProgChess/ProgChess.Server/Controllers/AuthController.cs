using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProChess.Server.Authorization;
using ProChess.Server.Entities;
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
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _authService.LoginAsync(request);
        if (result == null)
        {
            return BadRequest("Courriel ou mot de passe est invalide");
        }
        
        return Ok(result);
    }

    [HttpPost("login-code")]
    public async Task<IActionResult> LoginCode(StudentCodeDto request)
    {
        var success = await _authService.ContainsCodeAsync(request);
        if (!success)
        {
            return BadRequest("Code est invalide");
        }

        _cookieService.generateNormalCookie(Response, request);
        return Ok();
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshToken(RefreshTokenDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        if (result == null)
        {
            return Unauthorized("Refresh token invalid");
        }
        return Ok(result);
    }
    
    [HttpGet("verify-token")]
    [Authorize]
    public IActionResult VerifyToken()
    {
        return Ok();
    }
    
    [HttpGet("verify-cookie")]
    [ValidCodeCookie]
    public async Task<IActionResult> VerifyCookie([FromQuery] int exerciseId)
    {
        try
        {
            HttpContext.Request.Cookies.TryGetValue("studentCookie", out var studentCookie);
            var succes = await _authService.ContainsCodeAsync(new StudentCodeDto
            {
                ExerciseId = exerciseId,
                Code = studentCookie
            });
            if (!succes)
                return Unauthorized();
            return Ok();
        }
        catch (Exception e)
        {
            return Unauthorized("Étudiant non autorisé");
        }
    }
}