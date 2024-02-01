using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class LetterUserConfiguration : IEntityTypeConfiguration<LetterUser>
{
	public void Configure(EntityTypeBuilder<LetterUser> builder)
	{
		builder.Property(p => p.Firstname)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(p => p.Lastname)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(p => p.Title)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(p => p.RefreshToken)
			.HasMaxLength(64);

		builder.Property(p => p.Salutation)
			.IsRequired()
			.HasMaxLength(20);
	}
}
