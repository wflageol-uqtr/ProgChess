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
        try
        {
            // Faudrait tu que ce soit async ?
            var results = executeService.RunExerciseTest(request.Code, request.UnitTest);
            if (results.IsFailure)
            {
                return BadRequest(results.Error);
            }
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
            if (results != null && results.IsFailure)
            {
                return BadRequest(results.Error);
            }
            if (HttpContext.Request.Cookies.TryGetValue("studentCookie", out var studentCookie))
            {
                // Utilisation d'un builder pour reduire les arguments ou non nécessaire ?
                var score = await scoreService.AddScoreAsync(studentCookie, request.ExerciseId, request.Code, results.Value);
                if (score is null)
                    return BadRequest("Une erreur est survenue lors de la création du score");
                await exerciseService.RemoveStudentCode(request.ExerciseId, studentCookie);
                return Ok(new { score, results });
            }
            return BadRequest("Aucun étudiant spécifié");
        }
        catch (Exception e)
        {
            return BadRequest("Une erreur est survenue");
        }
    }
}