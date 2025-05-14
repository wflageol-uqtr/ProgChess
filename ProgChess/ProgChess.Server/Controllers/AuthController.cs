using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Entities;
using ProChess.Server.Models;
using ProChess.Server.Services;

namespace ProChess.Server.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService): ControllerBase
{

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var token = await authService.LoginAsync(request);
        if (token is null)
        {
            return BadRequest("Courriel ou mot de passe est invalide");
        }
        return Ok(token);
    }
}