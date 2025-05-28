using Microsoft.AspNetCore.Mvc;
using ProChess.Server.Authorization;

namespace ProChess.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[ValidCodeCookie]
public class ExecuteCodeController : ControllerBase
{
    [HttpPost]
    public Task<IActionResult> Execute()
    {
        
    }
}