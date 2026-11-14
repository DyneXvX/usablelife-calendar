using Microsoft.EntityFrameworkCore;

namespace UsableLifeCalendar.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
    }
}