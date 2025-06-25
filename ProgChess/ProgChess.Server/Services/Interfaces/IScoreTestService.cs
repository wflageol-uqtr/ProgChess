using ProChess.Server.Response;

namespace ProgChess.Server.Services;

public interface IScoreTestService
{
    Task Create(int scoreId, List<TestResult> testResults);
}