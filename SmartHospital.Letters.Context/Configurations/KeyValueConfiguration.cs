using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Context.Configurations;

public sealed class KeyValueConfiguration : IEntityTypeConfiguration<KeyValue>
{
	public void Configure(EntityTypeBuilder<KeyValue> builder)
	{
		builder.Property(p => p.Value)
			.IsRequired();

		builder.Property(p => p.ValueType)
			.IsRequired();

		builder.Property(p => p.Key)
			.IsRequired();

		builder.Property(p => p.KeyType)
			.IsRequired();
	}
}
