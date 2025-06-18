using ProChess.Server.Entities;

namespace ProgChess.Server.Services;

public interface IStudentExerciseService
{
    Task Create(int exerciseId, List<string> permanentCodes);
    
    Task Update(int exerciseId, List<string> permanentCodes);
}