using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Context.Configurations;

public class BaseEntityTypeConfiguration : IEntityTypeConfiguration<BaseClass>
{
	public void Configure(EntityTypeBuilder<BaseClass> builder)
	{
		builder.Property(p => p.Created)
			.IsRequired();
		builder.Property(p => p.CreatedBy)
			.IsRequired()
			.HasMaxLength(50);
		builder.Property(p => p.Modified)
			.IsRequired();
		builder.Property(p => p.ModifiedBy)
			.IsRequired()
			.HasMaxLength(50);
	}
}
