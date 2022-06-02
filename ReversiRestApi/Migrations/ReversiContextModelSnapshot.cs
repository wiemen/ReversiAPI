﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReversiRestApi;

namespace ReversiRestApi.Migrations
{
    [DbContext(typeof(ReversiContext))]
    partial class ReversiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ReversiRestApi.Models.Spel", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AandeBeurt")
                        .HasColumnType("int");

                    b.Property<string>("Beurten")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bord")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Omschrijving")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Speler1Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Speler2Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Spel");
                });

            modelBuilder.Entity("ReversiRestApi.Models.Uitslag", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PuntenWit")
                        .HasColumnType("int");

                    b.Property<int>("PuntenZwart")
                        .HasColumnType("int");

                    b.Property<Guid>("SpelID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Speler1Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Speler2Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Winnaar")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Uitslag");
                });
#pragma warning restore 612, 618
        }
    }
}