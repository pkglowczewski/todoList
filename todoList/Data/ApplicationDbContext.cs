using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using todoList.Models;

namespace todoList.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Priority> Priorities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Task>()
                .HasOne<ApplicationUser>(u => u.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Priority>().HasData(new Priority { PriorityId = 1, Name = "Bardzo wysoki"},
                new Priority { PriorityId = 2, Name = "Wysoki"},
                new Priority { PriorityId = 3, Name = "Średni"},
                new Priority { PriorityId = 4, Name = "Niski"});
        }
    }
}