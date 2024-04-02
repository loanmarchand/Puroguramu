using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class StatutExercicesConfiguration : IEntityTypeConfiguration<StatutExercice>
{
    public void Configure(EntityTypeBuilder<StatutExercice> builder) => builder.HasKey(statutExercice => statutExercice.IdStatutExercice);
}
