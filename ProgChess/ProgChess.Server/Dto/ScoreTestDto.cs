namespace ProgChess.Server.Dto;

public class ScoreTestDto
{
    public string Name { get; set; }
    
    public bool IsSuccess { get; set; }
    
    public string? Actual { get; set; }
    
    public string? Expected { get; set; }
}