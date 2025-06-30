using ProChess.Server.Entities;
using Microsoft.EntityFrameworkCore;
using ProChess.Server.Exceptions;
using ProgChess.Server.Database;

namespace ProgChess.Server.Services;

public class StudentExerciseService(AppDbContext dbContext, IStudentService studentService): IStudentExerciseService
{
    public async Task<List<StudentExercise>> GetStudentExercises()
    {
        return await dbContext.StudentExercises.ToListAsync();
    }

    public async Task Create(int exerciseId, List<string> permanentCodes)
    {
        var students = await CreateOrGetStudents(permanentCodes);
        var studentExercises = students.Select(student => new StudentExercise
        {
            Student = student,
            ExerciseId = exerciseId,
            IsComplete = false
        }).ToList();

        dbContext.StudentExercises.AddRange(studentExercises);
        await dbContext.SaveChangesAsync();
    }

    public async Task Update(int exerciseId, List<string> permanentCodes)
    {
        var studentExerciseList = await dbContext.StudentExercises
            .Where(se => se.ExerciseId == exerciseId)
            .ToListAsync();
        
        // Get Current student + create new
        var students = await CreateOrGetStudents(permanentCodes);

        // Get exisitng and new element
        var existingStudentIds = studentExerciseList.Select(se => se.StudentId).ToHashSet();
        var targetStudentIds = students.Select(s => s.Id).ToHashSet();
        
        // Remove student
        var studentsToRemove = studentExerciseList
            .Where(se => !targetStudentIds.Contains(se.StudentId))
            .ToList();
        dbContext.StudentExercises.RemoveRange(studentsToRemove);
        
        // Get new student
        var newStudentExercises = students
            .Where(s => !existingStudentIds.Contains(s.Id))
            .Select(s => new StudentExercise
            {
                ExerciseId = exerciseId,
                StudentId = s.Id,
                IsComplete = false
            })
            .ToList();

        dbContext.StudentExercises.AddRange(newStudentExercises);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateComplete(int exerciseId, string permanentCode)
    {
        var student = await studentService.GetStudentByPermanentCodeAsync(permanentCode);
        if (student == null)
            throw new NotFoundException("Étudiant introuvable");
        var result = await dbContext.StudentExercises.Where(se => se.ExerciseId == exerciseId && se.StudentId == student.Id)
            .FirstOrDefaultAsync();
        
        if (result == null)
            throw new NotFoundException("Exercice ou étudiant introuvable");
        result.IsComplete = true;
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateComplete((int exerciseId, int studentId) identifier, bool isComplete = false)
    {
        var result = await dbContext.StudentExercises.Where(se => se.ExerciseId == identifier.exerciseId && se.StudentId == identifier.studentId)
            .FirstOrDefaultAsync();
        if (result == null)
            throw new NotFoundException("Exercice ou étudiant introuvable");
        result.IsComplete = isComplete;
        await dbContext.SaveChangesAsync();
    }
    

    private async Task<List<Student>> CreateOrGetStudents(List<string> permanentCodes)
    {
        var result = new List<Student>();

        foreach (var permanentCode in permanentCodes)
        {
            var student = await dbContext.Students.FirstOrDefaultAsync(s => s.PermanentCode == permanentCode);
            if (student == null)
            {
                student = new Student { PermanentCode = permanentCode };
                dbContext.Students.Add(student);
                await dbContext.SaveChangesAsync();
            }
            result.Add(student);
        }
        return result;
    }
}