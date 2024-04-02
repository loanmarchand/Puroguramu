using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class ExercicesConfiguration : IEntityTypeConfiguration<Exercices>
{
    public void Configure(EntityTypeBuilder<Exercices> builder) => builder.HasKey(exercices => exercices.IdExercice);
}
