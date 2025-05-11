using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JeddahGreenHub.Models;

namespace JeddahGreenHub.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<JeddahGreenHub.Models.Article> Article { get; set; } = default!;

        public DbSet<JeddahGreenHub.Models.Comment> Comments { get; set; }
        public DbSet<JeddahGreenHub.Models.Event> Event { get; set; } = default!;
    }
}
