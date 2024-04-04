using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Puroguramu.Infrastructures.Configuration;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.data;

public class PurogumaruContext : IdentityDbContext<Utilisateurs>
{
    public PurogumaruContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Cours> Cours { get; set; }

    public DbSet<Lecons> Lecons { get; set; }

    public DbSet<Exercices> Exercices { get; set; }

    public DbSet<StatutExercice> StatutExercices { get; set; }

    public DbSet<PositionLecons> PositionLecons { get; set; }

    public DbSet<PositionExercices> PositionExercices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CoursConfiguration());
        modelBuilder.ApplyConfiguration(new LeconsConfiguration());
        modelBuilder.ApplyConfiguration(new ExercicesConfiguration());
        modelBuilder.ApplyConfiguration(new StatutExercicesConfiguration());
        modelBuilder.ApplyConfiguration(new PositionLeconsConfiguration());
        modelBuilder.ApplyConfiguration(new PositionExercicesConfiguration());
        modelBuilder.ApplyConfiguration(new UtilisateursConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
