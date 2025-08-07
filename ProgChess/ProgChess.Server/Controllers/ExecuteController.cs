using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Authorization;
using ProChess.Server.Exceptions;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExecuteController(ICodeExecuterService codeExecuterService, IScoreService scoreService, IStudentExerciseService studentExerciseService, IScoreTestService scoreTestService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Execute(ExecuteDto request)
    {
        var results = await codeExecuterService.ExecuteOnVm(request.Code, request.UnitTest);
        return Ok(results);
    }
    
    [HttpPost("submit")]
    [ValidCodeCookie]
    public async Task<IActionResult> Submit(ExecuteSubmitDto request)
    {
        var studentCookie = HttpContext.Items["studentCookie"] as string;
        if (string.IsNullOrEmpty(studentCookie))
            throw new UnauthorizedException("Cookie de l'étudiant non trouvé");
        var results = await codeExecuterService.ExecuteHiddenTestOnVm(request.Code, request.ExerciseId);
        var score = await scoreService.AddScoreAsync(studentCookie, request.ExerciseId, request.Code);
        await scoreTestService.Create(score, results.Value);
        await studentExerciseService.UpdateComplete(request.ExerciseId, studentCookie);
        return Ok();
    }
}