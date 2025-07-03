using ProChess.Server.Entities;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IExerciseService
{
    Task<int> Create(ExerciseDto request);
    Task<List<Exercise>?> GetAllExercice();
    Task<Exercise?> GetById(int id);
    Task<Exercise?> GetByIdWithActiveTest(int id, string studentCode);
    Task<Exercise?> GetByIdWithHiddenTest(int id);
    Task<int> Edit(int id, ExerciseDto request);
    Task Delete(int id);
    Task DeleteMultiple(DeleteMultipleDto request);
}