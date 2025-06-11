using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Authorization;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProgChess.Server.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService): ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(UserDto request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("login-code")]
    public async Task<IActionResult> LoginCode(StudentCodeDto request)
    {
        await _authService.LoginCodeAsync(request, Response);
        return Ok("Connexion réussi");
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<TokenDto>> RefreshToken(RefreshTokenDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
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
        var studentCookie = HttpContext.Items["studentCookie"] as string;
        
        await _authService.VerifyCodeAsync(new StudentCodeDto
        {
            ExerciseId = exerciseId,
            Code = studentCookie
        });
        return Ok();
    }
}