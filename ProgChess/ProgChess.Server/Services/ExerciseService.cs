using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProChess.Server.Utils;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class ExerciseService(AppDbContext dbContext) : IExerciseService
{
    private IExerciseService _exerciseServiceImplementation;

    public async Task<int> Create(ExerciseDto request)
    {
        var exercise = new Exercise
        {
            Situation = request.Situation,
            BaseCode = request.BaseCode,
            StudentCodes = Formatter.FormatCodeString(request.StudentCodes),
            UnitTests = request.UnitTest.Select(ut => new UnitTest
            {
                Code = ut.Code,
                IsActive = ut.IsActive,
            }).ToList()
        };
        await dbContext.Exercises.AddAsync(exercise);
        await dbContext.SaveChangesAsync();
        return exercise.Id;
    }

    public async Task<List<Exercise>?> GetAllExercice()
    {
        return await dbContext.Exercises.Include(e => e.UnitTests).ToListAsync();
    }

    public async Task<Exercise?> GetById(int id)
    {
        var exercise = await dbContext.Exercises.Include(e => e.UnitTests).FirstOrDefaultAsync(e => e.Id == id);
        if (exercise == null)
            throw new NotFoundException("Exercice introuvable");
        return exercise;
    }

    public async Task<Exercise?> GetByIdWithTestType(int id, bool isActive)
    {
        var exercise = await dbContext.Exercises.Include(e => e.UnitTests.Where(ut => ut.IsActive == isActive)).FirstOrDefaultAsync(e => e.Id == id);
        if (exercise == null)
            throw new NotFoundException("Exercice introuvable");
        return exercise;
    }

    public async Task<int> Edit(int id, ExerciseDto request)
    {
        // TODO: Marche pour le moment, c'est juste que je remove all et insert all pour le one-to-many, pas le best
        var exercise = await dbContext.Exercises.Where(e => e.Id == id).Include(e => e.UnitTests).FirstAsync();
        
        dbContext.Entry(exercise).State = EntityState.Detached;
        exercise.Situation = request.Situation;
        exercise.BaseCode = request.BaseCode;
        exercise.StudentCodes = Formatter.FormatCodeString(request.StudentCodes);
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
        return exercise.Id;
    }

    public async Task Delete(int id)
    {
         var exercise = await dbContext.Exercises.Include(e => e.UnitTests).FirstOrDefaultAsync(e => e.Id == id);
         if (exercise == null)
             throw new NotFoundException("Exercice introuvable");
         dbContext.Exercises.Remove(exercise);
         await dbContext.SaveChangesAsync();
    }

    public async Task RemoveStudentCode(int id, string permanentCode)
    {
        var exercise = await dbContext.Exercises.FirstOrDefaultAsync(e => e.Id == id);
        if (exercise == null)
            throw new NotFoundException("Exercice introuvable");
        exercise.StudentCodes.Remove(permanentCode);
        await dbContext.SaveChangesAsync();
    }
}