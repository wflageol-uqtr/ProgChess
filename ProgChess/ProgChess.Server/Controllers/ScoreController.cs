using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Authorization;
using ProChess.Server.Entities;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScoreController(IScoreService scoreService) : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetScoreById(int id)
    {
        var score = await scoreService.GetScoreByIdAsync(id);
        return Ok(score);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Score>>> GetScores()
    {
        var result = await scoreService.GetAllScores();
        return Ok(result);
    }
    
    
    [HttpGet("{id}/student-result")]
    [ValidCodeCookie]
    public async Task<IActionResult> GetScoreByExerciseAndStudent(int id)
    {
        var studentCookie = HttpContext.Items["studentCookie"] as string;
        
        var score = await scoreService.GetScoreByExerciseIdAndStudent(id, studentCookie);
        return Ok(score);
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<int>> Create(ScoreDto request)
    {
        var result = await scoreService.Create(request);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    [Authorize]
    public async Task<IActionResult> Edit([FromRoute] int id, ScoreDto request)
    {
        var result = await scoreService.Edit(id, request);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        await scoreService.Delete(id);
        return Ok("Score supprimé avec succès");
    }
}