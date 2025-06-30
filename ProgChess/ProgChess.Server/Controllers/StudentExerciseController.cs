using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Entities;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentExerciseController(IStudentExerciseService studentExerciseService): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<StudentExercise>>> GetAll()
    {
        var result = await studentExerciseService.GetStudentExercises();
        return Ok(result);
    }
}