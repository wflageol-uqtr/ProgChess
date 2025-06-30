using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class StudentDto
{
    [Required]
    public string PermanentCode { get; set; }
}