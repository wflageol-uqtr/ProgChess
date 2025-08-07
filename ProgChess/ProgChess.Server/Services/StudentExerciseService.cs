using ProChess.Server.Entities;
using Microsoft.EntityFrameworkCore;
using ProChess.Server.Context;
using ProChess.Server.Exceptions;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class StudentExerciseService(AppDbContext dbContext, IUserContext userContext): IStudentExerciseService
{
    public async Task<List<StudentExercise>> GetStudentExercises()
    {
        return await dbContext.StudentExercises.Include(se => se.Exercise).Where(se => se.Exercise.UserId == userContext.UserId).ToListAsync();
    }

    public async Task<StudentExercise> GetStudentExercisesById(int id)
    {
        var studentExercise = await dbContext.StudentExercises.FirstOrDefaultAsync(e => e.Id == id);
        if (studentExercise == null)
            throw new NotFoundException("Exercice introuvable");
        return studentExercise;
    }

    public async Task<StudentExercise> GetStudentExerciseByExerciseAndStudent(int id, string permanentCode)
    {
        var result = await dbContext.StudentExercises.FirstOrDefaultAsync(se => se.ExerciseId == id && se.StudentPermanentCode == permanentCode);
        if (result == null)
            throw new NotFoundException("Aucun exercise relié à l'étudiant trouvé");
        return result;
    }

    public async Task<List<StudentExerciseGroupDto>> GetGroupByStudentExercise()
    {
        return await dbContext.StudentExercises
            .Where(se => se.Exercise.UserId == userContext.UserId)
            .GroupBy(se => se.StudentPermanentCode)
            .Select(group => new StudentExerciseGroupDto
            {
                StudentPermanentCode = group.Key,
                Id = group.Select(s => s.Id).FirstOrDefault(),
                Exercises = group.Select(se => se.Exercise).ToList(),
                CreatedAt = group.Select(se => se.CreatedAt).FirstOrDefault(),
                UpdatedAt = group.Select(se => se.UpdatedAt).FirstOrDefault(),
            })
            .ToListAsync();
    }

    public async Task Create(int exerciseId, List<string> permanentCodes)
    {
        var studentExercises = permanentCodes.Select(permanentCode => new StudentExercise
        {
            StudentPermanentCode = permanentCode,
            ExerciseId = exerciseId,
            IsComplete = false
        }).ToList();

        dbContext.StudentExercises.AddRange(studentExercises);
        await dbContext.SaveChangesAsync();
    }

    public async Task EditPermanentCode(int id, StudentDto request)
    {
        var studentExercises = await dbContext.StudentExercises.Where(se => se.StudentPermanentCode == request.OldPermanentCode && se.Exercise.UserId == userContext.UserId).ToListAsync();
        if (studentExercises == null || studentExercises.Count == 0)
            throw new NotFoundException("Aucune donnée trouvée pour l'exercise");

        foreach (var se in studentExercises)
        {
            se.StudentPermanentCode = request.PermanentCode;
        }
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(int exerciseId, List<string> permanentCodes)
    {
        var studentExerciseList = await dbContext.StudentExercises
            .Where(se => se.ExerciseId == exerciseId)
            .ToListAsync();

        var existingPermanentCode = studentExerciseList.Select(se => se.StudentPermanentCode).ToHashSet();
        
        var studentsToRemove = studentExerciseList
            .Where(se => !permanentCodes.Contains(se.StudentPermanentCode))
            .ToList();
        
        dbContext.StudentExercises.RemoveRange(studentsToRemove);
        
        var newStudentExercises = permanentCodes
            .Where(code => !existingPermanentCode.Contains(code))
            .Select(code => new StudentExercise
            {
                ExerciseId = exerciseId,
                StudentPermanentCode = code,
                IsComplete = false
            })
            .ToList();

        dbContext.StudentExercises.AddRange(newStudentExercises);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateComplete(int exerciseId, string permanentCode)
    {
        var result = await dbContext.StudentExercises.Where(se => se.ExerciseId == exerciseId && se.StudentPermanentCode == permanentCode)
            .FirstOrDefaultAsync();
        
        if (result == null)
            throw new NotFoundException("Exercice ou étudiant introuvable");
        result.IsComplete = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateComplete((int exerciseId, int studentId) identifier, bool isComplete = false)
    {
        var result = await dbContext.StudentExercises.Where(se => se.ExerciseId == identifier.exerciseId)
            .FirstOrDefaultAsync();
        if (result == null)
            throw new NotFoundException("Exercice ou étudiant introuvable");
        result.IsComplete = isComplete;
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(string permanentCode)
    {
        var studentExercise = await dbContext.StudentExercises.Where(se => se.StudentPermanentCode == permanentCode && se.Exercise.UserId == userContext.UserId).ToListAsync();
        if (studentExercise == null || studentExercise.Count == 0)
            throw new NotFoundException("Aucun élément pour la suppression trouvé");
        dbContext.StudentExercises.RemoveRange(studentExercise);
        await dbContext.SaveChangesAsync();
    }
}