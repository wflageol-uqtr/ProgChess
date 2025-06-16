using ProChess.Server.Entities;
using ProChess.Server.Response;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IScoreService
{
    public Task<int> AddScoreAsync(string permanentCode, int exerciseId, string answer, List<TestResult> results);

    public Task<int> Create(ScoreDto request);

    public Task<List<Score>> GetAllScores();
    
    Task Delete(int id);
}