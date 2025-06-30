using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class ScoreDto
{
    [Required]
    public int ExerciseId { get; set; }
    
    [Required]
    public int StudentId { get; set; }
    
    public required string Answer { get; set; }
    
    public bool IsComplete { get; set; }
}