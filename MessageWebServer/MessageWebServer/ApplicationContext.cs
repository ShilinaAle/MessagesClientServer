using Microsoft.EntityFrameworkCore;

namespace MessageWebServer
{
    public class ApplicationContext : DbContext
    {  
        public DbSet<Message> Messages => Set<Message>();
        public ApplicationContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=messages.db");
        }
    }
}
