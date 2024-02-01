using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class LetterTypeConfiguration : IEntityTypeConfiguration<LetterType>
{
	public void Configure(EntityTypeBuilder<LetterType> builder)
	{
		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(50);
	}
}
