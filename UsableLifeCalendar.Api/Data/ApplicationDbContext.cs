using Microsoft.EntityFrameworkCore;
using UsableLifeCalendar.Api.Models;

namespace UsableLifeCalendar.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
    }
}