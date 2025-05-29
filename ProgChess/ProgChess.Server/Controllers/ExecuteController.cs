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
            return Ok(await executeService.RunExerciseTestAsync(request));
        }
        catch (Exception e)
        {
            return BadRequest("Une erreur est survenue");
        }
    }
}