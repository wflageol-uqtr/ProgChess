using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Entities;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScoreController(IScoreService scoreService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Score>>> GetScores()
    {
        var result = await scoreService.GetAllScores();
        return Ok(result);
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<Score>> Create(ScoreDto request)
    {
        var result = await scoreService.Create(request);
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