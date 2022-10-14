using Microsoft.EntityFrameworkCore;
using Parser1.Models;

namespace Parser1.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Direction> Directions { get; set; } = null!;
        public DbSet<Scientist> Scientists { get; set; } = null!;

        public DbSet<ScientistWork> ScientistsWork { get; set; } = null!;
        public DbSet<WorkOfScientist> WorkOfScientists { get; set; } = null!;

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql();
            //"Host=localhost;Port=5432;Database=papserDB;Username=postgres;Password=111116"
            //"Server=pasrerapi.postgres.database.azure.com;Database=postgres;Port=5432;User Id=Serg;Password=123456789Kk;Ssl Mode=Allow;"
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ScientistWork>()
                .HasKey(bc => new { bc.ScientistId, bc.WorkOfScientistId });

            modelBuilder.Entity<ScientistWork>()
                .HasOne(bc => bc.Scientist)
                .WithMany(b => b.ScientistsWorks)
                .HasForeignKey(bc => bc.ScientistId);

            modelBuilder.Entity<ScientistWork>()
                .HasOne(bc => bc.WorkOfScientist)
                .WithMany(c => c.ScientistsWorks)
                .HasForeignKey(bc => bc.WorkOfScientistId);
        }
    }
}
