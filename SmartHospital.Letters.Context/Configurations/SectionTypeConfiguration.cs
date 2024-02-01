using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class SectionTypeConfiguration : IEntityTypeConfiguration<SectionType>
{
	public void Configure(EntityTypeBuilder<SectionType> builder)
	{
		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(40);

		builder.Property(p => p.DefaultTitle)
			.IsRequired()
			.HasMaxLength(50);
	}
}
