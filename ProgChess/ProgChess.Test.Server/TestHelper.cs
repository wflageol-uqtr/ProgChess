using Microsoft.EntityFrameworkCore;
using ProgChess.Server.Database;

namespace ProgChess.Test.Server;

public static class TestHelper
{
    public static AppDbContext ContextGenerator()
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        return new AppDbContext(optionsBuilder.Options);
    }
}