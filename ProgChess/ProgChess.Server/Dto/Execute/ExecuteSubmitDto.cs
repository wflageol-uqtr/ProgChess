using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class ExecuteSubmitDto
{
    [Required]
    public required int ExerciseId { get; set; }
    
    public required string Code { get; set; }


}