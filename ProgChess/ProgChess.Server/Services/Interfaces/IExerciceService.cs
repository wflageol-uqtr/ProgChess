using ProChess.Server.Entities;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IExerciceService
{
    public Task<Exercice> Create(ExerciceDto request);
    Task<List<Exercice>?> GetAllExercice();
    Task<Exercice?> GetById(int id);
}