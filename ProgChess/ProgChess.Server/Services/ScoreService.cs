using ProChess.Server.Entities;
using ProChess.Server.Response;
using ProgChess.Server.Database;

namespace ProgChess.Server.Services;

public class ScoreService(AppDbContext context): IScoreService
{
    public async Task<int?> AddScoreAsync(string permanentCode, int exerciseId, string answer, List<TestResult> results)
    {
        try
        {
            var score = new Score
            {
                PermanentCode = permanentCode,
                ExerciseId = exerciseId,
                Answer = answer,
                ScoreValue = calculateScore(results),
            };
            await context.Scores.AddAsync(score);
            await context.SaveChangesAsync();
            return score.ScoreValue;
        }
        catch (Exception e)
        {
            return null;

        }
    }

    private int calculateScore(List<TestResult> testResults)
    {
        return testResults.OfType<TestSuccess>().Count();
    }
}