using Microsoft.EntityFrameworkCore;
using TaskFlowAPI.Model;

namespace TaskFlowAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TaskItem> Tasks { get; set; }
    }
}
