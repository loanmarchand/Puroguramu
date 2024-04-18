using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class UtilisateursConfiguration : IEntityTypeConfiguration<Utilisateurs>
{
    public void Configure(EntityTypeBuilder<Utilisateurs> builder)
    {
        builder.HasIndex(utilisateurs => utilisateurs.Matricule).IsUnique();
        SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<Utilisateurs> builder)
    {
        var hasher = new PasswordHasher<Utilisateurs>();
        var adminPassword = hasher.HashPassword(null!, "Romain1*");
        var admin = new Utilisateurs
        {
            Id = Guid.NewGuid().ToString(),
            Matricule = "P200000",
            Nom = "admin",
            Prenom = "admin",
            Email = "admin@example.com",
            Groupe = "0",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            UserName = "admin@example.com",
            NormalizedUserName = "ADMIN@EXAMPLE.COM",
            PasswordHash = adminPassword,
            SecurityStamp = Guid.NewGuid().ToString(),
            Role = Role.Teacher,
        };
        builder.HasData(admin);
    }
}
