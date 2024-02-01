namespace SmartHospital.Letters.Entities;

public class Section : BaseClass
{
	/// <summary>
	///     Used for Serialization/Deserialization
	/// </summary>
	public Section()
	{
	}


	public Section(Guid id, string title, SectionType sectionType, Letter letter,
		ICollection<Snippet> snippets,
		int sortOrder, DateTime created,
		string createdBy, DateTime modified = default, string modifiedBy = "")
		: base(id, created, createdBy, modified, modifiedBy)
	{
		Title = title;
		SectionType = sectionType;
		Letter = letter;
		Snippets = snippets;
		SortOrder = sortOrder;
	}

	public string Title { get; set; } = "";
	public SectionType SectionType { get; set; } = new();
	public Letter Letter { get; set; } = new();
	public ICollection<Snippet> Snippets { get; set; } = new List<Snippet>();
	public int SortOrder { get; set; }
}
