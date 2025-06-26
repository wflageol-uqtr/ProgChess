using ProChess.Server.Response;

namespace ProChess.Server.Entities;

public class Score: DateEntity
{
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    
    public Student Student { get; set; }
    
    public int ExerciseId { get; set; }
    
    public Exercise Exercise { get; set; }
    
    public int ScoreValue { get; set; }
    
    public string Answer { get; set; } = string.Empty;
    
    public ICollection<ScoreTest> ScoreTests { get; } = new List<ScoreTest>();

}