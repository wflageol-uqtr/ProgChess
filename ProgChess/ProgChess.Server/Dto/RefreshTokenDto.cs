namespace ProgChess.Server.Dto;

public class RefreshTokenDto
{
    public int Id { get; set; }
    public required string RefreshToken { get; set; }
}