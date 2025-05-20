using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Entities;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExerciceController(IExerciceService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Exercice>>> GetExercicesAsync()
    {
        var exercices = await service.GetAllExercice();
        return Ok(exercices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExerciceByIdAsync(int id)
    {
        var exercice = await service.GetById(id);
        if (exercice is null)
        {
            return NotFound("Not Found");
        }
        return Ok(exercice);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(ExerciceDto request)
    {
        var result = await service.Create(request);
        return Ok(result);
    }
}