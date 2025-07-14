using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Exceptions;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController: ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var images = Directory.GetFiles("Image")
            .Select(Path.GetFileName)
            .ToList();
        return Ok(images);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var path = Path.Combine("Image", $"{uniqueFileName}");
        await using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return Ok(new { filename = uniqueFileName });
    }

    [HttpDelete("{filename}")]
    [Authorize]
    public async Task<IActionResult> DeleteFile(string filename)
    {
        var path = Path.Combine("Image", $"{filename}");
        Console.WriteLine(path);
        Console.WriteLine(filename);
        if (!System.IO.File.Exists(path))
        {
            throw new NotFoundException("Image not found");
        }
        System.IO.File.Delete(path);
        return Ok();
    }
}