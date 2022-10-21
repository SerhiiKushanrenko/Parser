using Microsoft.EntityFrameworkCore;
using Parser1.Models;

namespace Parser1.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Direction> Directions { get; set; } = null!;
        public DbSet<Scientist> Scientists { get; set; } = null!;

        public DbSet<ScientistSubdirection> ScientistSubdirections { get; set; } = null!;
        public DbSet<ScientistWork> ScientistsWork { get; set; } = null!;
        public DbSet<Work> Works { get; set; } = null!;
        public DbSet<Subdirection> Subdirections { get; set; } = null!;

        public DbSet<SocialNetworkOfScientist> SocialNetworkOfScientists { get; set; } = null!;

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
                .HasOne(bc => bc.Scientist)
                .WithMany(b => b.ScientistsWorks)
                .HasForeignKey(bc => bc.ScientistId);

            modelBuilder.Entity<ScientistWork>()
                .HasOne(bc => bc.Work)
                .WithMany(c => c.ScientistsWorks)
                .HasForeignKey(bc => bc.WorkId);

            modelBuilder.Entity<Direction>()
                .HasMany(c => c.Subdirections)
                .WithOne(e => e.Direction);

            modelBuilder.Entity<SocialNetworkOfScientist>()
                .HasOne(s => s.Scientist)
                .WithMany(g => g.NetworkOfScientists)
                .HasForeignKey(s => s.ScientistId);

            modelBuilder.Entity<Scientist>()
                .HasOne(s => s.Organization)
                .WithMany(g => g.Scientists)
                .HasForeignKey(s => s.OrganizationId);

            modelBuilder.Entity<ScientistSubdirection>()
                .HasOne(bc => bc.Scientist)
                .WithMany(b => b.ScientistSubdirections)
                .HasForeignKey(bc => bc.ScientistId);

            modelBuilder.Entity<ScientistSubdirection>()
                .HasOne(bc => bc.Subdirection)
                .WithMany(c => c.ScientistSubdirections)
                .HasForeignKey(bc => bc.SubdirectionId);
        }
    }
}


