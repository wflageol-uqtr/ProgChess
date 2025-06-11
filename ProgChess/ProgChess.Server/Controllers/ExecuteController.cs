using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Authorization;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExecuteController(IExecuteService executeService, IScoreService scoreService, IExerciseService exerciseService) : ControllerBase
{
    [HttpPost]
    // [ValidCodeCookie]
    public async Task<IActionResult> Execute(ExecuteDto request)
    {
        //  Faudrait tu que ce soit async ?
        var results = executeService.RunExerciseTest(request.Code, request.UnitTest);
        return Ok(results);
    }
    
    [HttpPost("submit")]
    [ValidCodeCookie]
    public async Task<IActionResult> Submit(ExecuteSubmitDto request)
    {
        var studentCookie = HttpContext.Items["studentCookie"] as string;
        var results = await executeService.RunHiddenExerciseTestAsync(request.Code, request.ExerciseId);
        var score = await scoreService.AddScoreAsync(studentCookie, request.ExerciseId, request.Code, results.Value);
        await exerciseService.RemoveStudentCode(request.ExerciseId, studentCookie);
        return Ok(new { score, results });
    }
}