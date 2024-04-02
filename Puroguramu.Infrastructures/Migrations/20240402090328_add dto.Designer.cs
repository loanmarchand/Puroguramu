﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Puroguramu.Infrastructures.data;

#nullable disable

namespace Puroguramu.Infrastructures.Migrations
{
    [DbContext(typeof(PurogumaruContext))]
    [Migration("20240402090328_add dto")]
    partial class adddto
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.28");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.Exercices", b =>
                {
                    b.Property<string>("IdExercice")
                        .HasColumnType("TEXT");

                    b.Property<string>("Difficulte")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Enonce")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("EstVisible")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LeconsIdLecons")
                        .HasColumnType("TEXT");

                    b.Property<string>("Modele")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Solution")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdExercice");

                    b.HasIndex("LeconsIdLecons");

                    b.ToTable("Exercices");
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.Lecons", b =>
                {
                    b.Property<string>("IdLecons")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Titre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("estVisible")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdLecons");

                    b.HasIndex("Titre")
                        .IsUnique();

                    b.ToTable("Lecons");
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.PositionExercices", b =>
                {
                    b.Property<string>("IdPositionExercices")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExercicesIdExercice")
                        .HasColumnType("TEXT");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdPositionExercices");

                    b.HasIndex("ExercicesIdExercice");

                    b.ToTable("PositionExercices");
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.PositionLecons", b =>
                {
                    b.Property<string>("IdPositionLecons")
                        .HasColumnType("TEXT");

                    b.Property<string>("LeconsIdLecons")
                        .HasColumnType("TEXT");

                    b.Property<int>("Position")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdPositionLecons");

                    b.HasIndex("LeconsIdLecons");

                    b.ToTable("PositionLecons");
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.StatutExercice", b =>
                {
                    b.Property<string>("IdStatutExercice")
                        .HasColumnType("TEXT");

                    b.Property<string>("EtudiantId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExerciceIdExercice")
                        .HasColumnType("TEXT");

                    b.Property<string>("SolutionTempo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Statut")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdStatutExercice");

                    b.HasIndex("EtudiantId");

                    b.HasIndex("ExerciceIdExercice");

                    b.ToTable("StatutExercices");
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.Utilisateurs", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Groupe")
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("Matricule")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nom")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Prenom")
                        .HasColumnType("TEXT");

                    b.Property<int?>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Matricule")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Puroguramu.Infrastructures.dto.Utilisateurs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Puroguramu.Infrastructures.dto.Utilisateurs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Puroguramu.Infrastructures.dto.Utilisateurs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Puroguramu.Infrastructures.dto.Utilisateurs", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.Exercices", b =>
                {
                    b.HasOne("Puroguramu.Infrastructures.dto.Lecons", null)
                        .WithMany("ExercicesList")
                        .HasForeignKey("LeconsIdLecons");
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.PositionExercices", b =>
                {
                    b.HasOne("Puroguramu.Infrastructures.dto.Exercices", "Exercices")
                        .WithMany()
                        .HasForeignKey("ExercicesIdExercice");

                    b.Navigation("Exercices");
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.PositionLecons", b =>
                {
                    b.HasOne("Puroguramu.Infrastructures.dto.Lecons", "Lecons")
                        .WithMany()
                        .HasForeignKey("LeconsIdLecons");

                    b.Navigation("Lecons");
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.StatutExercice", b =>
                {
                    b.HasOne("Puroguramu.Infrastructures.dto.Utilisateurs", "Etudiant")
                        .WithMany()
                        .HasForeignKey("EtudiantId");

                    b.HasOne("Puroguramu.Infrastructures.dto.Exercices", "Exercice")
                        .WithMany()
                        .HasForeignKey("ExerciceIdExercice");

                    b.Navigation("Etudiant");

                    b.Navigation("Exercice");
                });

            modelBuilder.Entity("Puroguramu.Infrastructures.dto.Lecons", b =>
                {
                    b.Navigation("ExercicesList");
                });
#pragma warning restore 612, 618
        }
    }
}
