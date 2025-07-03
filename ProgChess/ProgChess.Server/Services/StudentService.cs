using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProgChess.Server.Database;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class StudentService(AppDbContext dbContext): IStudentService
{
    public Task<List<Student>> GetAllStudents()
    {
        return dbContext.Students.ToListAsync();
    }

    public async Task<Student> GetStudentByPermanentCodeAsync(string code)
    {
        var result = await dbContext.Students.FirstOrDefaultAsync(s => s.PermanentCode == code);
        if (result == null)
            throw new NotFoundException("Aucun étudiant à ce code permanent");
        return result;
    }

    public async Task<Student> GetStudentByIdAsync(int id)
    {
        var result = await dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
        if (result == null)
            throw new NotFoundException("Aucun étudiant trouvé");
        return result;    }

    public async Task<Student> Edit(int id, StudentDto request)
    {
        var student = await dbContext.Students.FirstOrDefaultAsync(s => s.Id == id);
        if (student == null)
            throw new NotFoundException("Aucun étudiant trouvé");
        dbContext.Entry(student).State = EntityState.Detached;
        student.PermanentCode = request.PermanentCode;
        dbContext.Students.Update(student);
        await dbContext.SaveChangesAsync();
        return student;
    }

    public async Task Delete(int id)
    {
        var student = await dbContext.Students.FindAsync(id);
        if (student == null)
            throw new NotFoundException("Student not found");
        dbContext.Students.Remove(student);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteMultiple(DeleteMultipleDto request)
    {
        var itemsToDelete = await dbContext.Students
            .Where(e => request.Ids.Contains(e.Id))
            .ToListAsync();

        if (itemsToDelete.Count == 0)
            throw new NotFoundException("Aucun élément à supprimer trouvé.");

        dbContext.Students.RemoveRange(itemsToDelete);
        await dbContext.SaveChangesAsync();
    }
}