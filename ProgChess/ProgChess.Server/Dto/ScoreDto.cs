using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class ScoreDto
{
    [Required]
    public int ExerciseId { get; set; }
    
    [Required]
    public int ScoreValue { get; set; }
    
    public required string PermanentCode { get; set; }
    
    public required string Answer { get; set; }
}