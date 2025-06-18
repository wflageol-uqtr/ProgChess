using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProChess.Server.Response;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class ScoreService(AppDbContext context, IStudentService studentService): IScoreService
{
    public async Task<int> AddScoreAsync(string permanentCode, int exerciseId, string answer, List<TestResult> results)
    {
        var student = await studentService.GetStudentByPermanentCodeAsync(permanentCode);
        var score = new Score
        {
            StudentId = student.Id,
            ExerciseId = exerciseId,
            Answer = answer,
            ScoreValue = CalculateScore(results),
        };
        await context.Scores.AddAsync(score);
        await context.SaveChangesAsync();
        return score.ScoreValue;
    }

    public async Task<int> Create(ScoreDto request)
    {
        // Validation nécessaire ?
        // var student = await studentService.GetStudentByIdAsync(request.StudentId);
        var score = new Score
        {
            StudentId = request.StudentId,
            ExerciseId = request.ExerciseId,
            Answer = request.Answer,
            ScoreValue = request.ScoreValue,
        };
        await context.Scores.AddAsync(score);
        await context.SaveChangesAsync();
        return score.ScoreValue; 
    }

    public async Task<Score> GetScoreByIdAsync(int id)
    {
        var result = await context.Scores.Include(s => s.Exercise).Include(s => s.Student).FirstOrDefaultAsync(s => s.Id == id);
        if (result == null)
            throw new NotFoundException("Aucun score trouvé");
        return result;
    }

    public async Task<List<Score>> GetAllScores()
    {
        return await context.Scores.Include(e => e.Exercise).Include(s => s.Student).ToListAsync();
    }

    public async Task<int> Edit(int id, ScoreDto request)
    {
        var score = await context.Scores.FirstOrDefaultAsync(s => s.Id == id);
        // var student = await studentService.GetStudentByPermanentCodeAsync(request.PermanentCode);
        if (score == null)
            throw new NotFoundException("Aucun score trouvé");
        
        context.Entry(score).State = EntityState.Detached;
        score.StudentId = request.StudentId;
        score.ExerciseId = request.ExerciseId;
        score.Answer = request.Answer;
        score.ScoreValue = request.ScoreValue;
        context.Scores.Update(score);
        await context.SaveChangesAsync();
        return score.Id;
    }

    public async Task Delete(int id)
    {
        var score = await context.Scores.FindAsync(id);
        if (score == null)
            throw new NotFoundException("Score not found");
        context.Scores.Remove(score);
        await context.SaveChangesAsync();
    }

    private int CalculateScore(List<TestResult> testResults)
    {
        return testResults.OfType<TestSuccess>().Count();
    }
}