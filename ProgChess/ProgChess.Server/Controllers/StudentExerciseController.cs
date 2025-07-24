using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Entities;
using ProgChess.Server.Dto;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentExercise>> GetById(int id)
    {
        var result = await studentExerciseService.GetStudentExercisesById(id);
        return Ok(result);
    }

    [HttpGet("group/student")]
    public async Task<ActionResult<List<StudentExerciseGroupDto>>> GetGroupByStudentExercise()
    {
        var result = await studentExerciseService.GetGroupByStudentExercise();
        return Ok(result);
    }
    
    [HttpPut("edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] int id, StudentDto request)
    {
        await studentExerciseService.EditPermanentCode(id, request);
        return Ok();
    }
    
    // Delete only the current user exercise sutdent at the code
    [HttpDelete("delete/{permanentCode}")]
    public async Task<IActionResult> Delete(string permanentCode)
    {
        await studentExerciseService.Delete(permanentCode);
        return Ok();
    }
}