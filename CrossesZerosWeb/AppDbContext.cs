using Microsoft.EntityFrameworkCore;

namespace CrossesZerosWeb
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<GameResult> GameResults { get; set; }
    }
}