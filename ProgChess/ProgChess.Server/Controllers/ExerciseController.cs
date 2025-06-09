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
    public async Task<ActionResult<List<Exercise>>> GetExercicesAsync()
    {
        var exercices = await service.GetAllExercice();
        return Ok(exercices);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetExerciceByIdAsync(int id)
    {
        var exercice = await service.GetById(id);
        if (exercice is null)
        {
            return NotFound("Not Found");
        }
        return Ok(exercice);
    }
    
    [HttpGet("active/{id}")]
    public async Task<IActionResult> GetExerciseByIdAsyncWithActiveTest(int id)
    {
        var exercice = await service.GetByIdWithTestType(id, true);
        if (exercice is null)
        {
            return NotFound("Not Found");
        }
        return Ok(exercice);
    }
    
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create(ExerciseDto request)
    {
        var result = await service.Create(request);
        if (result is null)
        {
            return StatusCode(500);
        }
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    [Authorize]
    public async Task<IActionResult> Edit([FromRoute] int id, ExerciseDto request)
    {
        var result = await service.Edit(id, request);
        if (result is null)
            return BadRequest("There is no such an exercise for id: " + id);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await service.Delete(id);
        if (!success)
        {
            return BadRequest("L'élément n'existe pas ");
        }

        return Ok("Exercice supprimé avec succès");
    }
}