using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class ExerciceService(AppDbContext dbContext) : IExerciceService
{
    public async Task<Exercice> Create(ExerciceDto request)
    {
        var exercice = dbContext.Exercices.Add(new Exercice
        {
            Situation = request.Situation,
            Code = request.Code,
        });
        
        await dbContext.SaveChangesAsync();
        return exercice.Entity;
    }

    public async Task<List<Exercice>?> GetAllExercice()
    {
        return await dbContext.Exercices.ToListAsync();
    }

    public async Task<Exercice?> GetById(int id)
    {
        var exercice = await dbContext.Exercices.FindAsync(id);
        return exercice ?? null;
    }
}