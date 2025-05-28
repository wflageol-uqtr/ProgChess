using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;
using ProChess.Server.Utils;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class ExerciseService(AppDbContext dbContext) : IExerciseService
{
    public async Task<int?> Create(ExerciseDto request)
    {
        try
        {
            var exercice = new Exercise
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
            await dbContext.Exercises.AddAsync(exercice);
            await dbContext.SaveChangesAsync();
            return exercice.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<List<Exercise>?> GetAllExercice()
    {
        try
        {
            return await dbContext.Exercises.Include(e => e.UnitTests).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<Exercise?> GetById(int id)
    {
        try
        {
            var exercice = await dbContext.Exercises.Include(e => e.UnitTests).FirstOrDefaultAsync(e => e.Id == id);
            return exercice ?? null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<int?> Edit(int id, ExerciseDto request)
    {
        try
        {
            // TODO: Marche pour le moment, c'est juste que je remove all et insert all pour le one-to-many, pas le best
            var exercice = await dbContext.Exercises.Where(e => e.Id == id).Include(e => e.UnitTests).FirstAsync();

            dbContext.Entry(exercice).State = EntityState.Detached;
            exercice.Situation = request.Situation;
            exercice.BaseCode = request.BaseCode;
            exercice.StudentCodes = Formatter.FormatCodeString(request.StudentCodes);
            dbContext.RemoveRange(exercice.UnitTests);
            dbContext.Exercises.Update(exercice);
            
            var newTest = new List<UnitTest>();
            foreach (var unitTest in request.UnitTest)
            {
                newTest.Add(new UnitTest
                {
                    Code = unitTest.Code,
                    IsActive = unitTest.IsActive,
                });
            }
                
            exercice.UnitTests = newTest;
            await dbContext.SaveChangesAsync();
            return exercice.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<bool> Delete(int id)
    {
        try
        {
             var exercice = await dbContext.Exercises.Include(e => e.UnitTests).FirstOrDefaultAsync(e => e.Id == id);
             if (exercice == null)
             {
                 return false;
             }
             dbContext.Exercises.Remove(exercice);
             await dbContext.SaveChangesAsync();
             return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;

        }
    }
}