using ProChess.Server.Entities.Interface;

namespace ProChess.Server.Entities;

public class UnitTest: DateEntity, ISoftDeletable
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int? ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}