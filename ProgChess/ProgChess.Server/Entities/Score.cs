using ProChess.Server.Entities.Interface;
using ProChess.Server.Response;

namespace ProChess.Server.Entities;

public class Score: DateEntity, ISoftDeletable
{
    public int Id { get; set; }
    
    public int StudentId { get; set; }
    
    public Student Student { get; set; }
    
    public int ExerciseId { get; set; }
    
    public Exercise Exercise { get; set; }
    
    public string Answer { get; set; } = string.Empty;
    
    public ICollection<ScoreTest> ScoreTests { get; set; } = new List<ScoreTest>();

    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}