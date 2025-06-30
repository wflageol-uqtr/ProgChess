using ProChess.Server.Entities;

namespace ProgChess.Server.Services;

public interface IStudentExerciseService
{
    Task<List<StudentExercise>> GetStudentExercises();
    Task Create(int exerciseId, List<string> permanentCodes);
    
    Task Update(int exerciseId, List<string> permanentCodes);
    
    Task UpdateComplete(int exerciseId, string permanentCode);

    Task UpdateComplete((int exerciseId, int studentId) identifier, bool isComplete = false);
}