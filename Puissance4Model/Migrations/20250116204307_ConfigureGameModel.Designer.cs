﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Puissance4Model.Data;

#nullable disable

namespace Puissance4Model.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250116204307_ConfigureGameModel")]
    partial class ConfigureGameModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("Puissance4Model.Models.Cell", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Column")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Row")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TokenId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TokenId");

                    b.ToTable("Cell");
                });

            modelBuilder.Entity("Puissance4Model.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GuestId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HostId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GuestId");

                    b.HasIndex("HostId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Puissance4Model.Models.Grid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Columns")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rows")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Grid");
                });

            modelBuilder.Entity("Puissance4Model.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Puissance4Model.Models.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Token");
                });

            modelBuilder.Entity("Puissance4Model.Models.Cell", b =>
                {
                    b.HasOne("Puissance4Model.Models.Grid", null)
                        .WithMany("Cells")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Puissance4Model.Models.Token", "Token")
                        .WithMany()
                        .HasForeignKey("TokenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Token");
                });

            modelBuilder.Entity("Puissance4Model.Models.Game", b =>
                {
                    b.HasOne("Puissance4Model.Models.Player", "Guest")
                        .WithMany("GamesAsGuest")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Puissance4Model.Models.Player", "Host")
                        .WithMany("GamesAsHost")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Puissance4Model.Models.Grid", "Grid")
                        .WithOne()
                        .HasForeignKey("Puissance4Model.Models.Game", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grid");

                    b.Navigation("Guest");

                    b.Navigation("Host");
                });

            modelBuilder.Entity("Puissance4Model.Models.Grid", b =>
                {
                    b.Navigation("Cells");
                });

            modelBuilder.Entity("Puissance4Model.Models.Player", b =>
                {
                    b.Navigation("GamesAsGuest");

                    b.Navigation("GamesAsHost");
                });
#pragma warning restore 612, 618
        }
    }
}
