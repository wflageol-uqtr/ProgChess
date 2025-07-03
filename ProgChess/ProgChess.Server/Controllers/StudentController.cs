using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Entities;
using ProgChess.Server.Dto;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentController(IStudentService studentService): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Student>>> GetStudents()
    {
        var result = await studentService.GetAllStudents();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudent(int id)
    {
        var result = await studentService.GetStudentByIdAsync(id);
        return Ok(result);
    }

    [HttpPut("edit/{id}")]
    public async Task<ActionResult<Student>> Edit(int id, StudentDto request)
    {
        var result = await studentService.Edit(id, request);
        return Ok(result);
    }

    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await studentService.Delete(id);
        return Ok();
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteAll(DeleteMultipleDto request)
    {
        await studentService.DeleteMultiple(request);
        return Ok();
    }
}