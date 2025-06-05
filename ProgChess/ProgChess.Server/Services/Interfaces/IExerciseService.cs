using ProChess.Server.Entities;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IExerciseService
{
    public Task<int?> Create(ExerciseDto request);
    Task<List<Exercise>?> GetAllExercice();
    Task<Exercise?> GetById(int id);
    Task<Exercise?> GetByIdWithHiddenTest(int id);
    Task<int?> Edit(int id, ExerciseDto request);
    Task<bool> Delete(int id);
    Task RemoveStudentCode(int id, string permanentCode);
}