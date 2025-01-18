﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Puissance4Model.Data;

#nullable disable

namespace Puissance4Model.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("Cell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Column")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GridId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Row")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TokenId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GridId");

                    b.HasIndex("TokenId");

                    b.ToTable("Cell");
                });

            modelBuilder.Entity("Puissance4Model.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
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

                    b.Property<int>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rows")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId")
                        .IsUnique();

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

            modelBuilder.Entity("Cell", b =>
                {
                    b.HasOne("Puissance4Model.Models.Grid", null)
                        .WithMany("Cells")
                        .HasForeignKey("GridId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Puissance4Model.Models.Token", "Token")
                        .WithMany()
                        .HasForeignKey("TokenId");

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

                    b.Navigation("Guest");

                    b.Navigation("Host");
                });

            modelBuilder.Entity("Puissance4Model.Models.Grid", b =>
                {
                    b.HasOne("Puissance4Model.Models.Game", null)
                        .WithOne("Grid")
                        .HasForeignKey("Puissance4Model.Models.Grid", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Puissance4Model.Models.Game", b =>
                {
                    b.Navigation("Grid")
                        .IsRequired();
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
