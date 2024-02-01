using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class LetterConfiguration : IEntityTypeConfiguration<Entities.Letter>
{
	public void Configure(EntityTypeBuilder<Entities.Letter> builder)
	{
		builder.Property(p => p.AdmissionType)
			.IsRequired();

		builder.Property(p => p.FirstName)
			.HasMaxLength(40);

		builder.Property(p => p.LastName)
			.HasMaxLength(40);

		builder.Property(p => p.ExternalCaseNumber)
			.HasMaxLength(12);

		builder.Property(p => p.ExternalPatientId)
			.IsRequired()
			.HasMaxLength(36);

		builder.Property(p => p.Status)
			.IsRequired();

		builder.HasMany(p => p.Sections)
			.WithOne(p => p.Letter);
	}
}
