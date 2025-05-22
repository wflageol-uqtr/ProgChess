using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class ExerciceService(AppDbContext dbContext) : IExerciceService
{
    public async Task<int?> Create(ExerciceDto request)
    {
        try
        {
            var exercice = new Exercice
            {
                Situation = request.Situation,
                Code = request.Code,
                UnitTests = request.UnitTest.Select(ut => new UnitTest
                {
                    Code = ut.Code,
                    IsActive = ut.IsActive,
                }).ToList()
            };
            await dbContext.Exercices.AddAsync(exercice);
            await dbContext.SaveChangesAsync();
            return exercice.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<List<Exercice>?> GetAllExercice()
    {
        try
        {
            return await dbContext.Exercices.Include(e => e.UnitTests).ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<Exercice?> GetById(int id)
    {
        try
        {
            var exercice = await dbContext.Exercices.Include(e => e.UnitTests).FirstOrDefaultAsync(e => e.Id == id);
            return exercice ?? null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<int?> Edit(int id, ExerciceDto request)
    {
        try
        {
            var exercice = await dbContext.Exercices.Where(e => e.Id == id).Include(e => e.UnitTests).FirstOrDefaultAsync();
            
            if (exercice == null)
                return null;
            
            dbContext.Entry(exercice).State = EntityState.Detached;
            exercice.Situation = request.Situation;
            exercice.Code = request.Code;
            
            // TODO: Fixer duplication des tests on save
            
            dbContext.Exercices.Update(exercice);
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
             var exercice = await dbContext.Exercices.Include(e => e.UnitTests).FirstOrDefaultAsync(e => e.Id == id);
             if (exercice == null)
             {
                 return false;
             }
             dbContext.Exercices.Remove(exercice);
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