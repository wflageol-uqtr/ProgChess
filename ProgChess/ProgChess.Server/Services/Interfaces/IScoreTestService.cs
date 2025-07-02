using ProChess.Server.Entities;
using ProChess.Server.Response;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IScoreTestService
{
    Task Create(int scoreId, List<TestResult> testResults);
    Task CreateFromAdmin(int scoreId, ICollection<ScoreTestDto> scoreTests);
    Task Edit(Score score, ICollection<ScoreTestDto> scoreTests);
}