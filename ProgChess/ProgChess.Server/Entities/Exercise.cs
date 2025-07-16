using System.Collections;

namespace ProChess.Server.Entities;

public class Exercise: DateEntity
{
    public int Id { get; set; }
    
    public string Situation { get; set; } = string.Empty;
    
    public string? BaseCode { get; set; } = string.Empty;
    
    public ICollection<StudentExercise> StudentExercises { get; set; } = new List<StudentExercise>();
    
    public ICollection<UnitTest> UnitTests { get; set; } = new List<UnitTest>();
}