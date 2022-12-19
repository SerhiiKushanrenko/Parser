﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(ParserDbContext))]
    [Migration("20221003082250_addtableScientistWork")]
    partial class addtableScientistWork
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Parser1.Models.Direction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Directions");
                });

            modelBuilder.Entity("Parser1.Models.Scientist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DirectionId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Organization")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DirectionId");

                    b.ToTable("Scientists");
                });

            modelBuilder.Entity("Parser1.Models.WorkOfScientist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("scientistId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("WorkOfScientists");
                });

            modelBuilder.Entity("ScientistWorkOfScientist", b =>
                {
                    b.Property<int>("ScientistsId")
                        .HasColumnType("integer");

                    b.Property<int>("WorkOfScientistsId")
                        .HasColumnType("integer");

                    b.HasKey("ScientistsId", "WorkOfScientistsId");

                    b.HasIndex("WorkOfScientistsId");

                    b.ToTable("ScientistWorkOfScientist");
                });

            modelBuilder.Entity("Parser1.Models.Scientist", b =>
                {
                    b.HasOne("Parser1.Models.Direction", "Direction")
                        .WithMany()
                        .HasForeignKey("DirectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Direction");
                });

            modelBuilder.Entity("ScientistWorkOfScientist", b =>
                {
                    b.HasOne("Parser1.Models.Scientist", null)
                        .WithMany()
                        .HasForeignKey("ScientistsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parser1.Models.WorkOfScientist", null)
                        .WithMany()
                        .HasForeignKey("WorkOfScientistsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
