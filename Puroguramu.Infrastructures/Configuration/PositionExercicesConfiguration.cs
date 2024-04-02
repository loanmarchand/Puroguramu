using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class PositionExercicesConfiguration : IEntityTypeConfiguration<PositionExercices>
{
    public void Configure(EntityTypeBuilder<PositionExercices> builder) => builder.HasKey(exercices => exercices.IdPositionExercices);
}
