using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProChess.Server.Response;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class ScoreService(AppDbContext context): IScoreService
{
    public async Task<int> AddScoreAsync(string permanentCode, int exerciseId, string answer, List<TestResult> results)
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
            throw new Exception(e.Message);
        }
    }

    public async Task<int> Create(ScoreDto request)
    {
        try
        {
            var score = new Score
            {
                PermanentCode = request.PermanentCode,
                ExerciseId = request.ExerciseId,
                Answer = request.Answer,
                ScoreValue = request.Score,
            };
            await context.Scores.AddAsync(score);
            await context.SaveChangesAsync();
            return score.ScoreValue;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }    }

    public async Task<List<Score>> GetAllScores()
    {
        try
        {
            return await context.Scores.Include(e => e.Exercise).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task Delete(int id)
    {
        try
        {
            var score = await context.Scores.FindAsync(id);
            if (score == null)
                throw new NotFoundException("Score not found");
            context.Scores.Remove(score);
            await context.SaveChangesAsync();
        }
        catch (NotFoundException e)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    private int calculateScore(List<TestResult> testResults)
    {
        return testResults.OfType<TestSuccess>().Count();
    }
}