using Microsoft.EntityFrameworkCore;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProgChess.Server.Database;

namespace ProgChess.Server.Services;

public class StudentService(AppDbContext dbContext): IStudentService
{
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
}