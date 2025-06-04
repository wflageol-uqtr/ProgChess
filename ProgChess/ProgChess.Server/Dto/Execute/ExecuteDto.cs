using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class ExecuteDto
{
    [Required]
    public required int ExerciseId { get; set; }
    
    public required string Code { get; set; }
    
    public required string UnitTest { get; set; }
}