using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController: ControllerBase
{
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
}