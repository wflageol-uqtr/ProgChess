using Microsoft.EntityFrameworkCore;
using ProChess.Server.Context;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProChess.Server.Response;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class ScoreService(AppDbContext context, IStudentService studentService, IStudentExerciseService studentExerciseService, IScoreTestService scoreTestService, IUserContext userContext): IScoreService
{
    public async Task<int> AddScoreAsync(string permanentCode, int exerciseId, string answer, List<TestResult> results)
    {
        var student = await studentService.GetStudentByPermanentCodeAsync(permanentCode);
        var score = new Score
        {
            StudentId = student.Id,
            ExerciseId = exerciseId,
            Answer = answer,
        };
        await context.Scores.AddAsync(score);
        await context.SaveChangesAsync();
        return score.Id;
    }

    public async Task<Score> Create(ScoreDto request)
    {
        var score = new Score
        {
            StudentId = request.StudentId,
            ExerciseId = request.ExerciseId,
            Answer = request.Answer,
        };
        await context.Scores.AddAsync(score);
        await context.SaveChangesAsync();
        await studentExerciseService.UpdateComplete((request.ExerciseId, request.StudentId));
        await scoreTestService.CreateFromAdmin(score.Id, request.ScoreTests);
        return score; 
    }

    public async Task<Score> GetScoreByIdAsync(int id)
    {
        var result = await context.Scores.Include(s => s.Exercise).ThenInclude(s => s.StudentExercises).Include(s => s.Student).Include(s => s.ScoreTests).FirstOrDefaultAsync(s => s.Id == id);
        if (result == null)
            throw new NotFoundException("Aucun score trouvé");
        return result;
    }

    public async Task<Score> GetScoreByExerciseIdAndStudent(int id, string studentCode)
    {
        var result = await context.Scores.Include(s => s.ScoreTests)
            .Where(s => s.Student.PermanentCode == studentCode && s.ExerciseId == id)
            .FirstOrDefaultAsync();
        if (result == null)
            throw new NotFoundException("Aucun score trouvé");
        return result;
    }

    public async Task<List<Score>> GetAllScores()
    {
        return await context.Scores.Include(e => e.Exercise).Where(s => s.Exercise.UserId == userContext.UserId)
            .Include(s => s.Student).ToListAsync();
    }

    public async Task<Score> Edit(int id, ScoreDto request)
    {
        var score = await context.Scores.Include(x => x.ScoreTests).FirstOrDefaultAsync(s => s.Id == id);
        if (score == null)
            throw new NotFoundException("Aucun score trouvé");
        
        context.Entry(score).State = EntityState.Detached;
        score.StudentId = request.StudentId;
        score.ExerciseId = request.ExerciseId;
        score.Answer = request.Answer;
        context.Scores.Update(score);
        await context.SaveChangesAsync();
        await studentExerciseService.UpdateComplete((request.ExerciseId, request.StudentId), request.IsComplete);
        await scoreTestService.Edit(score, request.ScoreTests);
        return score;
    }

    public async Task Delete(int id)
    {
        var score = await context.Scores.FindAsync(id);
        if (score == null)
            throw new NotFoundException("Score not found");
        context.Scores.Remove(score);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteMultiple(DeleteMultipleDto request)
    {
        var itemsToDelete = await context.Scores
            .Where(e => request.Ids.Contains(e.Id))
            .ToListAsync();

        if (itemsToDelete.Count == 0)
            throw new NotFoundException("Aucun élément à supprimer trouvé.");

        context.Scores.RemoveRange(itemsToDelete);
        await context.SaveChangesAsync();
    }
}