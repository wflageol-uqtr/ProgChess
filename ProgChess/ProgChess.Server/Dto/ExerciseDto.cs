using System.ComponentModel.DataAnnotations;
using ProChess.Server.Entities;

namespace ProgChess.Server.Dto;

public class ExerciseDto
{
    [Required]
    public required string Situation { get; set; }
    public string? BaseCode { get; set; }
    
    public required string StudentCodes { get; set; }
    
    [Required]
    [MinLength(1)]
    public required ICollection<UnitTest> UnitTest { get; set; }
}