using Microsoft.EntityFrameworkCore;
using ProChess.Server.Context;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProChess.Server.Response;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class ScoreService(AppDbContext context, IStudentExerciseService studentExerciseService, IScoreTestService scoreTestService, IUserContext userContext): IScoreService
{
    public async Task<int> AddScoreAsync(string permanentCode, int exerciseId, string answer)
    {
        var student = await studentExerciseService.GetStudentExerciseByExerciseAndStudent(exerciseId, permanentCode);
        var existingScore = await context.Scores
            .Where(s => s.ExerciseId == exerciseId && s.StudentExercise.StudentPermanentCode == permanentCode)
            .Include(score => score.ScoreTests).
            FirstOrDefaultAsync();
        if (existingScore != null)
        {
            context.RemoveRange(existingScore.ScoreTests);
            context.Remove(existingScore);
            await context.SaveChangesAsync();
        }
        var score = new Score
        {
            StudentExerciseId = student.Id,
            ExerciseId = exerciseId,
            Answer = answer,
        };
        await context.Scores.AddAsync(score);
        await context.SaveChangesAsync();
        return score.Id;
        
    }

    public async Task<Score> Create(ScoreDto request)
    {
        var existingScore = await context.Scores
            .Where(s => s.ExerciseId == request.ExerciseId && s.StudentExerciseId == request.StudentExerciseId)
            .Include(score => score.ScoreTests).
            FirstOrDefaultAsync();
        if (existingScore != null)
        {
            context.RemoveRange(existingScore.ScoreTests);
            context.Remove(existingScore);
            await context.SaveChangesAsync();
        }
        var score = new Score
        {
            StudentExerciseId = request.StudentExerciseId,
            ExerciseId = request.ExerciseId,
            Answer = request.Answer,
        };
        await context.Scores.AddAsync(score);
        await context.SaveChangesAsync();
        await studentExerciseService.UpdateComplete((request.ExerciseId, request.StudentExerciseId), request.IsComplete);
        await scoreTestService.CreateFromAdmin(score.Id, request.ScoreTests);
        return score; 
    }

    public async Task<Score> GetScoreByIdAsync(int id)
    {
        var result = await context.Scores.Include(s => s.Exercise).ThenInclude(s => s.StudentExercises).Include(s => s.ScoreTests).FirstOrDefaultAsync(s => s.Id == id);
        if (result == null)
            throw new NotFoundException("Score introuvable");
        return result;
    }

    public async Task<Score> GetScoreByExerciseIdAndStudent(int id, string studentCode)
    {
        var result = await context.Scores.Include(s => s.ScoreTests)
            .Where(s => s.StudentExercise.StudentPermanentCode == studentCode && s.ExerciseId == id)
            .FirstOrDefaultAsync();
        if (result == null)
            throw new NotFoundException("Score introuvable");
        return result;
    }

    public async Task<List<Score>> GetAllScores()
    {
        return await context.Scores.Include(e => e.Exercise).Where(s => s.Exercise.UserId == userContext.UserId)
            .Include(s => s.StudentExercise).ToListAsync();
    }

    public async Task<Score> Edit(int id, ScoreDto request)
    {
        var score = await context.Scores.Include(x => x.ScoreTests).FirstOrDefaultAsync(s => s.Id == id);
        if (score == null)
            throw new NotFoundException("Score introuvable");
        
        context.Entry(score).State = EntityState.Detached;
        score.StudentExerciseId = request.StudentExerciseId;
        score.ExerciseId = request.ExerciseId;
        score.Answer = request.Answer;
        context.Scores.Update(score);
        await context.SaveChangesAsync();
        await studentExerciseService.UpdateComplete((request.ExerciseId, request.StudentExerciseId), request.IsComplete);
        await scoreTestService.Edit(score, request.ScoreTests);
        return score;
    }

    public async Task Delete(int id)
    {
        var score = await context.Scores.Include(s => s.ScoreTests).FirstOrDefaultAsync(s => s.Id == id);
        if (score == null)
            throw new NotFoundException("Score introuvable");
        context.Scores.Remove(score);
        context.ScoreTest.RemoveRange(score.ScoreTests);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteMultiple(DeleteMultipleDto request)
    {
        var scores = await context.Scores
            .Include(s => s.ScoreTests)
            .Where(e => request.Ids.Contains(e.Id))
            .ToListAsync();

        if (scores.Count == 0)
            throw new NotFoundException("Aucun élément à supprimer trouvé");

        context.Scores.RemoveRange(scores);
        context.ScoreTest.RemoveRange(scores.SelectMany(s => s.ScoreTests).ToList());
        await context.SaveChangesAsync();
    }
}