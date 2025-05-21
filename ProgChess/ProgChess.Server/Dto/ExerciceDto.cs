using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class ExerciceDto
{
    [Required]
    public required string Situation { get; set; }
    
    public string? Code { get; set; }
}