using Microsoft.EntityFrameworkCore;
using ProChess.Server.Context;
using ProChess.Server.Entities;
using ProChess.Server.Exceptions;
using ProgChess.Server.Database;

namespace ProgChess.Server.Services;

public class UploadService(AppDbContext context, IUserContext userContext): IUploadService
{
    public async Task<List<Image>> GetAll()
    {
        return await context.Images.Where(x => x.UserId == userContext.UserId).ToListAsync();
    }
    
    public async Task<Image> Save(IFormFile file)
    {
        
        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var path = Path.Combine("Image", $"{uniqueFileName}");
        await using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var image = new Image
        {
            UserId = userContext.UserId,
            Name = file.FileName,
            Path = path
        };
        context.Images.Add(image);
        await context.SaveChangesAsync();
        return image;
    }

    public async Task Delete(int id)
    {
        var image = await context.Images.FirstOrDefaultAsync(i => i.Id == id);
        if (image == null)
            throw new NotFoundException("Image introuvable");
            
        if (!File.Exists(image.Path))
        {
            throw new NotFoundException("Image introuvable");
        }
        context.Images.Remove(image);
        await context.SaveChangesAsync();
        File.Delete(image.Path);
    }
}