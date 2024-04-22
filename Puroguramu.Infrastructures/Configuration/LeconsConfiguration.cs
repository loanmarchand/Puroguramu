using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class LeconsConfiguration : IEntityTypeConfiguration<Lecons>
{
    public void Configure(EntityTypeBuilder<Lecons> builder)
    {
        builder.HasKey(lecons => lecons.IdLecons);
        builder.HasIndex(lecons => lecons.Titre)
            .IsUnique();

        builder.HasMany(lecon => lecon.ExercicesList)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
