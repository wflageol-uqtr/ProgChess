namespace ProgChess.Server.Dto;

public class VMExecuteDto
{
    public bool IsSuccess { get; set; }
    
    public string? Error { get; set; }
    
    public string? Output { get; set; }
}