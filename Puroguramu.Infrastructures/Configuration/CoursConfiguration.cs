using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class CoursConfiguration : IEntityTypeConfiguration<Cours>
{
    public void Configure(EntityTypeBuilder<Cours> builder)
    {
        builder.HasKey(cours => cours.IdCours);
        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<Cours> builder) =>
        builder.HasData(
            new Cours
            {
                IdCours = "1",
                Titre = "C#",
                ImageUrl = "~/images/csharp.svg",
            });
}
