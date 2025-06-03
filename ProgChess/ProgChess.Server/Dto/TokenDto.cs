namespace ProgChess.Server.Dto;

public class TokenDto
{
    public required string UserId { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}