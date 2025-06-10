using ProChess.Server.Entities;
using ProChess.Server.Response;

namespace ProgChess.Server.Services;

public interface IScoreService
{
    public Task<int?> AddScoreAsync(string permanentCode, int exerciseId, string answer, List<TestResult> results);

    public Task<List<Score>> GetAllScores();
    
    Task<bool> Delete(int id);
}