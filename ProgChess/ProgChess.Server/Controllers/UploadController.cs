using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Exceptions;
using ProgChess.Server.Services;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController(IUploadService uploadService): ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await uploadService.GetAll();
        return Ok(result);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        var image = await uploadService.Save(file);
        return Ok(new { image });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteFile(int id)
    {
        await uploadService.Delete(id);
        return Ok("Image supprimé avec succès !");
    }
}