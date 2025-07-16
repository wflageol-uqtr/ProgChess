using System.ComponentModel.DataAnnotations;
using ProChess.Server.Entities.Interface;

namespace ProChess.Server.Entities;

public class Student: DateEntity, ISoftDeletable
{
    [Key]
    public int Id { get; set; }
    
    public string PermanentCode { get; set; }
    
    public ICollection<StudentExercise> StudentExercises { get; set; } = new List<StudentExercise>();
    
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}