using ProChess.Server.Entities;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IExerciceService
{
    public Task<int?> Create(ExerciceDto request);
    Task<List<Exercice>?> GetAllExercice();
    Task<Exercice?> GetById(int id);
    Task<int?> Edit(int id, ExerciceDto request);
    Task<bool> Delete(int id);
}