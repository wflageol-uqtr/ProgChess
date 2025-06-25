using System.ComponentModel.DataAnnotations;

namespace ProgChess.Server.Dto;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}