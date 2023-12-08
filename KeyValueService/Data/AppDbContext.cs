using KeyValueService.Models;
using Microsoft.EntityFrameworkCore;

namespace KeyValueService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<KeyValue> KeyValues { get; set; }
    }
}
