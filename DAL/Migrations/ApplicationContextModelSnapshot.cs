﻿// <auto-generated />
using System;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(ParserDbContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAL.Models.FieldOfResearch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ANZSRC")
                        .HasColumnType("integer");

                    b.Property<int?>("ParentFieldOfResearchId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ParentFieldOfResearchId");

                    b.ToTable("FieldOfResearch");
                });

            modelBuilder.Entity("DAL.Models.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("DAL.Models.Scientist", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Degree")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("OrganizationId")
                        .HasColumnType("integer");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Scientists");
                });

            modelBuilder.Entity("DAL.Models.ScientistFieldOfResearch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FieldOfResearchId")
                        .HasColumnType("integer");

                    b.Property<int>("ScientistId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FieldOfResearchId");

                    b.HasIndex("ScientistId");

                    b.ToTable("ScientistFieldOfResearch");
                });

            modelBuilder.Entity("DAL.Models.ScientistSocialNetwork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ScientistId")
                        .HasColumnType("integer");

                    b.Property<string>("SocialNetworkScientistId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScientistId");

                    b.ToTable("SocialNetworkOfScientists");
                });

            modelBuilder.Entity("DAL.Models.ScientistWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ScientistId")
                        .HasColumnType("integer");

                    b.Property<int>("WorkId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ScientistId");

                    b.HasIndex("WorkId");

                    b.ToTable("ScientistsWork");
                });

            modelBuilder.Entity("DAL.Models.Work", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Works");
                });

            modelBuilder.Entity("DAL.Models.FieldOfResearch", b =>
                {
                    b.HasOne("DAL.Models.FieldOfResearch", "ParentFieldOfResearch")
                        .WithMany("ChildFieldsOfResearch")
                        .HasForeignKey("ParentFieldOfResearchId");

                    b.Navigation("ParentFieldOfResearch");
                });

            modelBuilder.Entity("DAL.Models.Scientist", b =>
                {
                    b.HasOne("DAL.Models.Organization", "Organization")
                        .WithMany("Scientists")
                        .HasForeignKey("OrganizationId");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("DAL.Models.ScientistFieldOfResearch", b =>
                {
                    b.HasOne("DAL.Models.FieldOfResearch", "FieldOfResearch")
                        .WithMany("ScientistsFieldsOfResearch")
                        .HasForeignKey("FieldOfResearchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.Scientist", "Scientist")
                        .WithMany("ScientistFieldsOfResearch")
                        .HasForeignKey("ScientistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FieldOfResearch");

                    b.Navigation("Scientist");
                });

            modelBuilder.Entity("DAL.Models.ScientistSocialNetwork", b =>
                {
                    b.HasOne("DAL.Models.Scientist", "Scientist")
                        .WithMany("ScientistSocialNetworks")
                        .HasForeignKey("ScientistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scientist");
                });

            modelBuilder.Entity("DAL.Models.ScientistWork", b =>
                {
                    b.HasOne("DAL.Models.Scientist", "Scientist")
                        .WithMany("ScientistsWorks")
                        .HasForeignKey("ScientistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Models.Work", "Work")
                        .WithMany("ScientistsWorks")
                        .HasForeignKey("WorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Scientist");

                    b.Navigation("Work");
                });

            modelBuilder.Entity("DAL.Models.FieldOfResearch", b =>
                {
                    b.Navigation("ChildFieldsOfResearch");

                    b.Navigation("ScientistsFieldsOfResearch");
                });

            modelBuilder.Entity("DAL.Models.Organization", b =>
                {
                    b.Navigation("Scientists");
                });

            modelBuilder.Entity("DAL.Models.Scientist", b =>
                {
                    b.Navigation("ScientistFieldsOfResearch");

                    b.Navigation("ScientistSocialNetworks");

                    b.Navigation("ScientistsWorks");
                });

            modelBuilder.Entity("DAL.Models.Work", b =>
                {
                    b.Navigation("ScientistsWorks");
                });
#pragma warning restore 612, 618
        }
    }
}
