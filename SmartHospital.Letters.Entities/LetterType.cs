namespace SmartHospital.Letters.Entities;

public class LetterType : BaseClass
{
	public LetterType()
	{
	}

	public LetterType(
		string name,
		DateTime created,
		string createdBy,
		Guid? id = null,
		DateTime modified = default,
		string modifiedBy = ""
	) : base(id, created, createdBy, modified, modifiedBy)
	{
		Name = name;
	}

	public string Name { get; set; } = "";
}
