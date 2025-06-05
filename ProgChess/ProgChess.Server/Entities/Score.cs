namespace ProChess.Server.Entities;

public class Score
{
    public int Id { get; set; }
    
    public string PermanentCode { get; set; } = string.Empty;
    
    public int ExerciseId { get; set; }
    
    public Exercise Exercise { get; set; }
    
    public int ScoreValue { get; set; }
    
    public string Answer { get; set; } = string.Empty;
}