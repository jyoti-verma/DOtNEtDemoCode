namespace SmartHospital.Letters.Entities;

public class Snippet : BaseClass
{
	/// <summary>
	///     Used for Serialization/Deserialization
	/// </summary>
	public Snippet()
	{
	}

	/// <summary>
	///     Creates a new instance of <see cref="Snippet" />
	/// </summary>
	/// <param name="id"></param>
	/// <param name="title"></param>
	/// <param name="section"></param>
	/// <param name="sortOrder"></param>
	/// <param name="keyValues"></param>
	/// <param name="created"></param>
	/// <param name="createdBy"></param>
	/// <param name="modified"></param>
	/// <param name="modifiedBy"></param>
	public Snippet(
		Guid id,
		string title,
		Section section,
		int sortOrder,
		ICollection<KeyValue> keyValues,
		DateTime created,
		string createdBy,
		DateTime modified = default,
		string modifiedBy = ""
	)
		: base(id, created, createdBy, modified, modifiedBy)
	{
		Title = title;
		Section = section;
		SortOrder = sortOrder;
		KeyValues = keyValues;
	}

	public string Title { get; set; } = "";
	public Section Section { get; set; } = new();
	public ICollection<KeyValue> KeyValues { get; set; } = new List<KeyValue>();
	public int SortOrder { get; set; }
}
