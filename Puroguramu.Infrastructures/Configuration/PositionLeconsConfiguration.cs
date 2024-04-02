using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Puroguramu.Infrastructures.dto;

namespace Puroguramu.Infrastructures.Configuration;

public class PositionLeconsConfiguration : IEntityTypeConfiguration<PositionLecons>
{
    public void Configure(EntityTypeBuilder<PositionLecons> builder) => builder.HasKey(positionLecons => positionLecons.IdPositionLecons);
}
