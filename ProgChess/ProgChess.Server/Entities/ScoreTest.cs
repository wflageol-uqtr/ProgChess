namespace ProChess.Server.Entities;

public class ScoreTest: DateEntity
{
    public int Id { get; set; }
    
    public int ScoreId { get; set; }
    
    public Score Score { get; set; }
    
    public string Name { get; set; }
    
    public bool IsSuccess { get; set; }
    
    public string? Actual { get; set; }
    
    public string? Expected { get; set; }
}