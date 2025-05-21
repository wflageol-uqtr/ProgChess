using System.ComponentModel.DataAnnotations;
using ProChess.Server.Entities;

namespace ProgChess.Server.Dto;

public class ExerciceDto
{
    [Required]
    public required string Situation { get; set; }
    public string? Code { get; set; }
    
    [Required]
    [MinLength(1)]
    public required ICollection<UnitTest> UnitTest { get; set; }
}