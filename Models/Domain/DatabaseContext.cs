using CalendarEvents.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Models.Domain
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la relación entre ApplicationUser y CalendarEvent
            modelBuilder.Entity<CalendarEvent>()
                .HasOne(c => c.Lawyer)
                .WithMany(u => u.CalendarEvents)
                .HasForeignKey(c => c.LawyerId);
        }
    }
}
