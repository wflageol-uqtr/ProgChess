using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Entities;
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
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await scoreService.Delete(id);
        if (!success)
        {
            return BadRequest("L'élément n'existe pas ");
        }

        return Ok("Score supprimé avec succès");
    }
}