using System.ComponentModel.DataAnnotations;
using ProChess.Server.Entities;

namespace ProgChess.Server.Dto;

public class ScoreDto
{
    [Required]
    public int ExerciseId { get; set; }
    
    [Required]
    public int StudentId { get; set; }
    
    public required string Answer { get; set; } = string.Empty;
    
    public bool IsComplete { get; set; }
    
    public ICollection<ScoreTestDto> ScoreTests { get; set; }
}