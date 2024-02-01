using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class SnippetConfiguration : IEntityTypeConfiguration<Snippet>
{
	public void Configure(EntityTypeBuilder<Snippet> builder)
	{
		builder.Property(p => p.Title)
			.IsRequired()
			.HasMaxLength(200);

		builder.HasMany(p => p.KeyValues)
			.WithOne(p => p.Snippet);
	}
}
