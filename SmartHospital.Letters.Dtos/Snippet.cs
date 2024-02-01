using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Dtos;

public class Snippet : IIdentifier<Guid>
{
	public string Title { get; set; } = null!;
	public ICollection<KeyValue> KeyValues { get; set; } = new List<KeyValue>();
	public int SortOrder { get; set; }
	public Guid Id { get; set; } = Guid.NewGuid();
}
