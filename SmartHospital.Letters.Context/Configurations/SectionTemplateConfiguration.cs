using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class SectionTemplateConfiguration : IEntityTypeConfiguration<SectionTemplate>
{
	public void Configure(EntityTypeBuilder<SectionTemplate> builder)
	{
		builder.HasOne(p => p.SectionType);

		builder.HasMany(p => p.SnippetTemplates)
			.WithOne(p => p.SectionTemplate)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
	}
}
