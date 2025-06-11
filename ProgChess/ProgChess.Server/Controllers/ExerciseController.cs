using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Entities;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseController(IExerciseService service) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Exercise>>> GetExercisesAsync()
    {
        var exercises = await service.GetAllExercice();
        return Ok(exercises);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetExerciseByIdAsync(int id)
    {
        var exercise = await service.GetById(id);
        return Ok(exercise);
    }
    
    [HttpGet("active/{id}")]
    public async Task<IActionResult> GetExerciseByIdAsyncWithActiveTest(int id)
    {
        var exercise = await service.GetByIdWithTestType(id, true);
        return Ok(exercise);
    }
    
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create(ExerciseDto request)
    {
        var result = await service.Create(request);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    [Authorize]
    public async Task<IActionResult> Edit([FromRoute] int id, ExerciseDto request)
    {
        var result = await service.Edit(id, request);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        await service.Delete(id);
        return Ok("Exercice supprimé avec succès");
    }
}