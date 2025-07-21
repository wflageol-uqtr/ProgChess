using System.Collections;
using ProChess.Server.Entities.Interface;

namespace ProChess.Server.Entities;

public class Exercise: DateEntity, ISoftDeletable
{
    public int Id { get; set; }
    
    public string Situation { get; set; } = string.Empty;
    
    public string? BaseCode { get; set; } = string.Empty;
    
    public string UserId { get; set; }
    
    public User User { get; set; } = null!;
    
    public ICollection<StudentExercise> StudentExercises { get; set; } = new List<StudentExercise>();
    
    public ICollection<UnitTest> UnitTests { get; set; } = new List<UnitTest>();
    
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}