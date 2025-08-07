using ProChess.Server.Entities;
using ProChess.Server.Response;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IScoreService
{
    Task<int> AddScoreAsync(string permanentCode, int exerciseId, string answer);

    Task<Score> Create(ScoreDto request);
    
    Task<Score> GetScoreByIdAsync(int id);
    Task<Score> GetScoreByExerciseIdAndStudent(int id, string studentCode);
    Task<List<Score>> GetAllScores();
    Task<Score> Edit(int id, ScoreDto request);
    Task Delete(int id);
    Task DeleteMultiple(DeleteMultipleDto request);
}