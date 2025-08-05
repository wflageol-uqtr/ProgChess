using ProChess.Server.Entities.Interface;

namespace ProChess.Server.Entities;

public class ScoreTest: DateEntity, ISoftDeletable
{
    public int Id { get; set; }
    
    public int ScoreId { get; set; }
    
    public Score Score { get; set; }
    
    public string Name { get; set; }
    
    public bool IsSuccess { get; set; }
    
    public string? Actual { get; set; }
    
    public string? Expected { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}