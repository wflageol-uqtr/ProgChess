namespace ProgChess.Server.Dto;

public class RefreshTokenDto
{
    public required string UserId { get; set; }
    public required string RefreshToken { get; set; }
}