using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProgChess.VM.Simulation;

[ApiController]
[Route("[controller]")]
public class VmController(VmService vmService): ControllerBase
{
    [HttpPost("execute")]
    public async Task<ActionResult<string>> Post([FromBody] string code)
    {
        var response = await vmService.ExecuteAsync(code);
        return Ok(response);
    }
}