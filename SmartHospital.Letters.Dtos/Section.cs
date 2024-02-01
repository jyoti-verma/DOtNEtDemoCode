using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Dtos;

public class Section : IIdentifier<Guid>
{
	public string Title { get; set; } = null!;
	public Guid SectionTypeId { get; set; }
	public int SortOrder { get; set; }
	public ICollection<Snippet> Snippets { get; set; } = new List<Snippet>();
	public Guid Id { get; set; }
}
