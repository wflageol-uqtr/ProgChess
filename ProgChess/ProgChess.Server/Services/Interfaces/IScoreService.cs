using ProChess.Server.Response;

namespace ProgChess.Server.Services;

public interface IScoreService
{
    public Task<int?> AddScoreAsync(string permanentCode, int exerciseId, string answer, List<TestResult> results);
}