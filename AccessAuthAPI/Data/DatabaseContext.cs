using AccessAuthAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessAuthAPI.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
