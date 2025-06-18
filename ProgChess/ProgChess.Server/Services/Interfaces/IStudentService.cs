using ProChess.Server.Entities;

namespace ProgChess.Server.Services;

public interface IStudentService
{
    Task<Student> GetStudentByPermanentCodeAsync(string code);
}