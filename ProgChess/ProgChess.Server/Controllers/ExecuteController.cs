using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Authorization;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExecuteController(IExecuteService executeService) : ControllerBase
{
    [HttpPost]
    [ValidCodeCookie]
    public async Task<IActionResult> Execute(ExecuteDto request)
    {
        try
        {
            // Faudrait tu que ce soit async ?
            var results = executeService.RunVisibleExerciseTestAsync(request.Code, request.UnitTest, request.ExerciseId);
            if (results is null)
                return BadRequest("Une erreur est survenue");
            return Ok(results);
        }
        catch (Exception e)
        {
            return BadRequest("Une erreur est survenue");
        }
    }
    
    [HttpPost("submit")]
    [ValidCodeCookie]
    public async Task<IActionResult> Submit(ExecuteSubmitDto request)
    {
        try
        {
            var results = await executeService.RunHiddenExerciseTestAsync(request.Code, request.ExerciseId);
            if (results is null)
                return BadRequest("Une erreur est survenue");
            return Ok(results);
        }
        catch (Exception e)
        {
            return BadRequest("Une erreur est survenue");
        }
    }
}