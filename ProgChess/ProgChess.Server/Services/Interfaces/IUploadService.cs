using ProChess.Server.Entities;

namespace ProgChess.Server.Services;

public interface IUploadService
{
    Task<List<Image>> GetAll();
    Task<Image> Save(IFormFile file);
    Task Delete(int id);
}