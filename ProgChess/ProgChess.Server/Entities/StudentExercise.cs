using ProChess.Server.Entities.Interface;

namespace ProChess.Server.Entities;

public class StudentExercise: DateEntity, ISoftDeletable
{
    public int Id { get; set; }
    public string StudentPermanentCode { get; set; }
    public int ExerciseId { get; set; }
    
    public Exercise Exercise { get; set; }
    public bool IsComplete{ get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}