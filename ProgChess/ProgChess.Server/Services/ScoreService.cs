using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Score>> GetAllScores()
    {
        try
        {
            return await context.Scores.Include(e => e.Exercise).ToListAsync();
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
            var score = await context.Scores.FindAsync(id);
            if (score == null)
            {
                return false;
            }
            context.Scores.Remove(score);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    private int calculateScore(List<TestResult> testResults)
    {
        return testResults.OfType<TestSuccess>().Count();
    }
}