using ProChess.Server.Entities;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IStudentExerciseService
{
    Task<List<StudentExercise>> GetStudentExercises();
    Task<StudentExercise> GetStudentExercisesById(int id);
    Task<StudentExercise> GetStudentExerciseByExerciseAndStudent(int id, string permanentCode);
    Task<List<StudentExerciseGroupDto>> GetGroupByStudentExercise();
    Task Create(int exerciseId, List<string> permanentCodes);
    Task EditPermanentCode(int id, StudentDto request);
    Task Update(int exerciseId, List<string> permanentCodes);
    
    Task UpdateComplete(int exerciseId, string permanentCode);

    Task UpdateComplete((int exerciseId, int studentId) identifier, bool isComplete = false);
    Task Delete(string permanentCode);
}