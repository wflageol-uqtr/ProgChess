using System.ComponentModel.DataAnnotations;

namespace ProChess.Server.Entities;

public class Student
{
    [Key]
    public int Id { get; set; }
    
    public string PermanentCode { get; set; }
    
    public ICollection<StudentExercise> StudentExercises { get; set; } = new List<StudentExercise>();
}