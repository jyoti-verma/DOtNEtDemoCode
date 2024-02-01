using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities.Templates;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class LetterTemplateSectionTemplateConfiguration : IEntityTypeConfiguration<LetterTemplateSectionTemplate>
{
	public void Configure(EntityTypeBuilder<LetterTemplateSectionTemplate> builder)
	{
		builder.HasIndex(p => new { p.SortOrder, p.Id });

		builder.Property(p => p.SortOrder)
			.IsRequired();
	}
}
