using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class UtilisateursConfiguration : IEntityTypeConfiguration<Utilisateurs>
{
    public void Configure(EntityTypeBuilder<Utilisateurs> builder) => builder.HasIndex(utilisateurs => utilisateurs.Matricule).IsUnique();
}
