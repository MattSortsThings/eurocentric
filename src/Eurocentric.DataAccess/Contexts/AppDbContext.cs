using Microsoft.EntityFrameworkCore;

namespace Eurocentric.DataAccess.Contexts;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
