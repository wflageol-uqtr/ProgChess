using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class StudentCodeDto
{
    [Required]
    [MinLength(12)]
    [MaxLength(12)]
    public required string Code { get; set; }
    
    [Required]
    public required int ExerciceId { get; set; }
}