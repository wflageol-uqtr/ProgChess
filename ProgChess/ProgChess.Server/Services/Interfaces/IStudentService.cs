using ProChess.Server.Entities;
using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface IStudentService
{
    Task<List<Student>> GetAllStudents();
    Task<Student> GetStudentByPermanentCodeAsync(string code);
    Task<Student> GetStudentByIdAsync(int id);
    Task<Student> Edit(int id, StudentDto request);
    Task Delete(int id);
    Task DeleteMultiple(DeleteMultipleDto request);
}