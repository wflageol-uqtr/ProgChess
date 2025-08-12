using Microsoft.EntityFrameworkCore;
using ProChess.Server.Context;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProChess.Server.Utils;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class ExerciseService(AppDbContext dbContext, IStudentExerciseService studentExerciseService, IUserContext userContext, IScoreService scoreService): IExerciseService
{
    private IExerciseService _exerciseServiceImplementation;

    public async Task<int> Create(ExerciseDto request)
    {
        if (userContext.UserId is null)
            throw new UnauthorizedException("L'utilisateur n'est pas authentifié");
        
        var exercise = new Exercise
        {
            Situation = request.Situation,
            BaseCode = request.BaseCode,
            UserId = userContext.UserId,
            UnitTests = request.UnitTest.Select(ut => new UnitTest
            {
                Code = ut.Code,
                IsActive = ut.IsActive,
            }).ToList()
        };
        await dbContext.Exercises.AddAsync(exercise);
        await dbContext.SaveChangesAsync();
        await studentExerciseService.Create(exercise.Id, Formatter.FormatCodeString(request.StudentCodes));
        return exercise.Id;
    }

    public async Task<List<Exercise>?> GetAllExercice()
    {
        return await dbContext.Exercises.Include(e => e.UnitTests).Include(e => e.StudentExercises).Where(x => x.UserId == userContext.UserId).ToListAsync();
    }

    public async Task<Exercise?> GetById(int id)
    {
        var exercise = await dbContext.Exercises.Include(e => e.UnitTests).Include(e => e.StudentExercises).FirstOrDefaultAsync(e => e.Id == id);
        if (exercise == null)
            throw new NotFoundException("Exercice introuvable");
        return exercise;
    }

    public async Task<Exercise?> GetByIdWithActiveTest(int id, string studentCode)
    {
        var exercise = await dbContext.Exercises.Include(e => e.UnitTests.Where(ut => ut.IsActive))
            .Include(e => e.StudentExercises.Where(se => se.StudentPermanentCode == studentCode))
            .FirstOrDefaultAsync(e => e.Id == id);
        if (exercise == null)
            throw new NotFoundException("Exercice introuvable");
        return exercise;
    }
    
    public async Task<Exercise?> GetByIdWithHiddenTest(int id)
    {
        var exercise = await dbContext.Exercises.Include(e => e.UnitTests.Where(ut => !ut.IsActive)).Include(e => e.StudentExercises).FirstOrDefaultAsync(e => e.Id == id);
        if (exercise == null)
            throw new NotFoundException("Exercice introuvable");
        return exercise;
    }

    public async Task<int> Edit(int id, ExerciseDto request)
    {
        if (userContext.UserId is null)
            throw new UnauthorizedException("L'utilisateur n'est pas authentifié");
        
        var exercise = await dbContext.Exercises.Where(e => e.Id == id).Include(e => e.UnitTests)
            .Include(e => e.StudentExercises)
            .FirstAsync();
        dbContext.Entry(exercise).State = EntityState.Detached;
        exercise.Situation = request.Situation;
        exercise.BaseCode = request.BaseCode;
        exercise.UserId = userContext.UserId;
        dbContext.RemoveRange(exercise.UnitTests);
        dbContext.Exercises.Update(exercise);
        
        var newTest = new List<UnitTest>();
        foreach (var unitTest in request.UnitTest)
        {
            newTest.Add(new UnitTest
            {
                Code = unitTest.Code,
                IsActive = unitTest.IsActive,
            });
        }
            
        exercise.UnitTests = newTest;
        await dbContext.SaveChangesAsync();
        await studentExerciseService.Update(id, Formatter.FormatCodeString(request.StudentCodes));
        return exercise.Id;
    }

    public async Task Delete(int id)
    {
         var exercise = await dbContext.Exercises.Include(e => e.UnitTests)
             .Include(e => e.StudentExercises)
             .Include(e => e.Scores)
             .ThenInclude(s => s.ScoreTests)
             .FirstOrDefaultAsync(e => e.Id == id);
         if (exercise == null)
             throw new NotFoundException("Exercice introuvable");
         dbContext.Exercises.Remove(exercise);
         dbContext.UnitTests.RemoveRange(exercise.UnitTests);
         dbContext.StudentExercises.RemoveRange(exercise.StudentExercises);
         dbContext.ScoreTest.RemoveRange(exercise.Scores.SelectMany(s => s.ScoreTests));
         dbContext.Scores.RemoveRange(exercise.Scores);
         await dbContext.SaveChangesAsync();
    }

    public async Task DeleteMultiple(DeleteMultipleDto request)
    {
        var exercises = await dbContext.Exercises
            .Include(e => e.UnitTests)
            .Include(e => e.StudentExercises)
            .Include(e => e.Scores)
            .ThenInclude(s => s.ScoreTests)
            .Where(e => request.Ids.Contains(e.Id))
            .ToListAsync();

        if (exercises.Count == 0)
            throw new NotFoundException("Aucun élément à supprimer trouvé.");

        dbContext.UnitTests.RemoveRange(exercises.SelectMany(e => e.UnitTests).ToList());
        dbContext.StudentExercises.RemoveRange(exercises.SelectMany(e => e.StudentExercises).ToList());
        dbContext.Exercises.RemoveRange(exercises);
        dbContext.Scores.RemoveRange(exercises.SelectMany(e => e.Scores).ToList());
        dbContext.ScoreTest.RemoveRange(
            exercises
                .SelectMany(e => e.Scores)
                .SelectMany(s => s.ScoreTests)
                .ToList()
        );        await dbContext.SaveChangesAsync();
    }
}