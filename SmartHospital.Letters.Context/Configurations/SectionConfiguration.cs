using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class SectionConfiguration : IEntityTypeConfiguration<Section>
{
	public void Configure(EntityTypeBuilder<Section> builder)
	{
		builder.HasIndex(p => new { p.SortOrder, p.Id });

		builder.Property(p => p.SortOrder)
			.IsRequired();

		builder.HasOne(p => p.SectionType);

		builder.HasMany(p => p.Snippets)
			.WithOne(p => p.Section);
	}
}
