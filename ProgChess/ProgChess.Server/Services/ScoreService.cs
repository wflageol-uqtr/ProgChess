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
                ScoreValue = request.ScoreValue,
            };
            await context.Scores.AddAsync(score);
            await context.SaveChangesAsync();
            return score.ScoreValue;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }    }

    public async Task<Score> GetScoreByIdAsync(int id)
    {
        try
        {
            var result = await context.Scores.Include(s => s.Exercise).FirstOrDefaultAsync(s => s.Id == id);
            if (result == null)
                throw new NotFoundException("Aucun score trouvé");
            return result;
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

    public async Task<int> Edit(int id, ScoreDto request)
    {
        try
        {
            var score = await context.Scores.FirstOrDefaultAsync(s => s.Id == id);
            if (score == null)
                throw new NotFoundException("Aucun score trouvé");
            
            context.Entry(score).State = EntityState.Detached;
            score.PermanentCode = request.PermanentCode;
            score.ExerciseId = request.ExerciseId;
            score.Answer = request.Answer;
            score.ScoreValue = request.ScoreValue;
            context.Scores.Update(score);
            await context.SaveChangesAsync();
            return score.Id;
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