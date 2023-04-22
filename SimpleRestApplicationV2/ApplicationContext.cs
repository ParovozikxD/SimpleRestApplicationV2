using Microsoft.EntityFrameworkCore;

namespace SimpleRestApplicationV2
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> dbContextOptions) 
            : base(dbContextOptions) 
        {
            Database.EnsureCreated();
        }

    }
}
