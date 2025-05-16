using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class StudentDto
{
    [Required]
    [MinLength(12)]
    [MaxLength(12)]
    public required string Code { get; set; }
}