using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class ExecuteDto
{
    public required string Code { get; set; }
    
    public required string UnitTest { get; set; }
}