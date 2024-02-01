namespace SmartHospital.Letters.Entities;

public class SectionType : BaseClass
{
	public SectionType()
	{
	}

	public SectionType(
		string name, string defaultTitle, Guid? id,
		DateTime created, string createdBy,
		DateTime modified = default, string modifiedBy = ""
	) : base(id, created, createdBy, modified, modifiedBy)
	{
		Name = name;
		DefaultTitle = defaultTitle;
	}

	public string Name { get; set; } = "";

	public string DefaultTitle { get; set; } = "";
}
