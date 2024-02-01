namespace SmartHospital.Letters.Entities.Templates;

public sealed class LetterTemplate : BaseClass
{
	/// <summary>
	///     Used for Serialization/Deserialization
	/// </summary>
	public LetterTemplate()
	{
	}

	public LetterTemplate(
		DateTime created,
		string createdBy,
		LetterType letterType,
		Guid? id = null,
		ICollection<SectionTemplate>? sectionTemplates = null,
		ICollection<LetterTemplateSectionTemplate>? letterTemplatesSectionTemplates = null,
		DateTime modified = default,
		string modifiedBy = ""
	) : base(id, created, createdBy, modified, modifiedBy)
	{
		Id = id ?? Guid.NewGuid();
		LetterType = letterType;
		SectionTemplates = sectionTemplates ?? new List<SectionTemplate>();
		LetterTemplatesSectionTemplates = letterTemplatesSectionTemplates ?? new List<LetterTemplateSectionTemplate>();
	}

	public ICollection<LetterTemplateSectionTemplate> LetterTemplatesSectionTemplates { get; set; }
		= new List<LetterTemplateSectionTemplate>();

	public LetterType LetterType { get; set; } = new();
	public ICollection<SectionTemplate> SectionTemplates { get; set; } = new List<SectionTemplate>();
}
