using ProChess.Server.Entities;

namespace ProgChess.Server.Dto;

public class StudentExerciseGroupDto
{
    public int Id { get; set; }
    public string StudentPermanentCode { get; set; }
    public List<Exercise> Exercises { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}