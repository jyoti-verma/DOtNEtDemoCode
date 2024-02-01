using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities.Templates;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class SnippetTemplateConfiguration : IEntityTypeConfiguration<SnippetTemplate>
{
	public void Configure(EntityTypeBuilder<SnippetTemplate> builder)
	{
		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(30);

		builder.Property(p => p.DataType)
			.IsRequired()
			.HasMaxLength(20);
	}
}
