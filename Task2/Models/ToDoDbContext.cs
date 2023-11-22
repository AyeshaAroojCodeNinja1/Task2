using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task2.Authentication;

namespace Task2.Models
{
    public class ToDoDbContext : IdentityDbContext<ApplicationUser>
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
        { }

        public DbSet<ToDo> ToDo { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
