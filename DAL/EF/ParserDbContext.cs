using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class ParserDbContext : DbContext
    {
        public DbSet<Direction> Directions { get; set; } = null!;
        public DbSet<Scientist> Scientists { get; set; } = null!;

        public DbSet<ScientistSubdirection> ScientistSubdirections { get; set; } = null!;
        public DbSet<ScientistWork> ScientistsWork { get; set; } = null!;
        public DbSet<Work> Works { get; set; } = null!;
        public DbSet<Subdirection> Subdirections { get; set; } = null!;

        public DbSet<SocialNetworkOfScientist> SocialNetworkOfScientists { get; set; } = null!;

        public ParserDbContext()
        {
        }

        public ParserDbContext(DbContextOptions<ParserDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql();
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


