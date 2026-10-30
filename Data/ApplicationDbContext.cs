using Microsoft.EntityFrameworkCore;
using FormApp.Models;

namespace FormApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Submission> Submissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Submission>(entity =>
            {
                entity.ToTable("submissions");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired();

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .IsRequired();

                entity.Property(e => e.Phone)
                    .HasColumnName("phone");

                entity.Property(e => e.Message)
                    .HasColumnName("message");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at");
            });
        }
    }
}