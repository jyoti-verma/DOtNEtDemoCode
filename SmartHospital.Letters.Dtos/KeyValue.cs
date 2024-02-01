using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Dtos;

public class KeyValue : IIdentifier<Guid>
{
	public string? Value { get; set; } = "";
	public string ValueType { get; set; } = null!;
	public string Key { get; set; } = null!;
	public string KeyType { get; set; } = null!;
	public int SortOrder { get; set; }
	public Guid Id { get; set; } = Guid.NewGuid();
}
