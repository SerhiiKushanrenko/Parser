﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using DAL.EF;

#nullable disable

namespace Parser.Migrations
{
        

    [DbContext(typeof(ParserDbContext))]
    [Migration("20220922084108_Initial")]
    partial class Initial
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

                    b.Property<int?>("DirectionId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Organization")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DirectionId");

                    b.ToTable("Scientists");
                });

            modelBuilder.Entity("Parser1.Models.Scientist", b =>
                {
                    b.HasOne("Parser1.Models.Direction", null)
                        .WithMany("Scientists")
                        .HasForeignKey("DirectionId");
                });

            modelBuilder.Entity("Parser1.Models.Direction", b =>
                {
                    b.Navigation("Scientists");
                });
#pragma warning restore 612, 618
        }
    }
}
