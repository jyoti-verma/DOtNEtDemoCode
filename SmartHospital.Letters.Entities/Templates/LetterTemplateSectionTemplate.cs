namespace SmartHospital.Letters.Entities.Templates;

public class LetterTemplateSectionTemplate : BaseClass
{
	public LetterTemplateSectionTemplate()
	{
	}

	public LetterTemplateSectionTemplate(
		int sortOrder,
		DateTime created,
		string createdBy,
		LetterTemplate letterTemplate,
		SectionTemplate sectionTemplate,
		Guid? id = null,
		DateTime modified = default,
		string modifiedBy = ""
	) : base(id, created, createdBy, modified, modifiedBy)
	{
		LetterTemplateId = letterTemplate.Id;
		SectionTemplateId = sectionTemplate.Id;
		SortOrder = sortOrder;
		LetterTemplate = letterTemplate;
		SectionTemplate = sectionTemplate;
	}

	public LetterTemplate LetterTemplate { get; set; } = new();
	public Guid LetterTemplateId { get; set; }
	public SectionTemplate SectionTemplate { get; set; } = new();
	public Guid SectionTemplateId { get; set; }
	public int SortOrder { get; set; }
}
