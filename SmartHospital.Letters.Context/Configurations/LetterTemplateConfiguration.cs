using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Entities.Templates;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class LetterTemplateConfiguration : IEntityTypeConfiguration<LetterTemplate>
{
	public void Configure(EntityTypeBuilder<LetterTemplate> builder)
	{
		builder.HasOne(p => p.LetterType);

		builder.HasMany(p => p.SectionTemplates)
			.WithMany(p => p.LetterTemplates)
			.UsingEntity<LetterTemplateSectionTemplate>(
				l =>
					l.HasOne<SectionTemplate>(e => e.SectionTemplate)
						.WithMany(e => e.LetterTemplateSectionTemplates),
				r =>
					r.HasOne<LetterTemplate>(e => e.LetterTemplate)
						.WithMany(e => e.LetterTemplatesSectionTemplates));
	}
}
