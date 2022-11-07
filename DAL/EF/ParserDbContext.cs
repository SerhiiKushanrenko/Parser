using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF
{
    public class ParserDbContext : DbContext
    {
        public DbSet<Scientist> Scientists { get; set; } = null!;
        public DbSet<ScientistFieldOfResearch> ScientistFieldOfResearch { get; set; } = null!;
        public DbSet<ScientistWork> ScientistsWork { get; set; } = null!;
        public DbSet<Work> Works { get; set; } = null!;
        public DbSet<ScientistFieldOfResearch> FieldOfResearch { get; set; } = null!;
        public DbSet<ScientistSocialNetwork> SocialNetworkOfScientists { get; set; } = null!;

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

            modelBuilder.Entity<ScientistSocialNetwork>()
                .HasOne(s => s.Scientist)
                .WithMany(g => g.ScientistSocialNetworks)
                .HasForeignKey(s => s.ScientistId);

            modelBuilder.Entity<Scientist>()
                .HasOne(s => s.Organization)
                .WithMany(g => g.Scientists)
                .HasForeignKey(s => s.OrganizationId);

            modelBuilder.Entity<Concept>()
                .HasOne(s => s.Scientist)
                .WithMany(g => g.Concepts)
                .HasForeignKey(s => s.ScientistId);

            //modelBuilder.Entity<ScientistFieldOfResearch>()
            //    .HasMany(s => s.ScientistsFieldsOfResearch)
            //    .WithOne(g => g.FieldOfResearch)
            //    .HasForeignKey(s => s.FieldOfResearchId);

            //modelBuilder.Entity<ScientistFieldOfResearch>()
            //    .HasOne(s => s.ParentFieldOfResearch)
            //    .WithMany(g => g.ChildFieldsOfResearch)
            //    .HasForeignKey(s => s.ParentFieldOfResearchId);

            modelBuilder.Entity<ScientistFieldOfResearch>()
                .HasOne(s => s.Scientist)
                .WithMany(g => g.ScientistFieldsOfResearch)
                .HasForeignKey(s => s.ScientistId);

            modelBuilder.Entity<ScientistFieldOfResearch>()
                .HasOne(s => s.FieldOfResearch)
                .WithMany(g => g.ScientistsFieldsOfResearch)
                .HasForeignKey(s => s.FieldOfResearchId);
        }
    }
}


