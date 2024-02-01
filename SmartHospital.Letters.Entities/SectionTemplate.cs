using SmartHospital.Letters.Entities.Templates;

namespace SmartHospital.Letters.Entities;

public class SectionTemplate : BaseClass
{
	/// <summary>
	///     Used for Deserialization/Serilization
	/// </summary>
	public SectionTemplate()
	{
	}

	public SectionTemplate(SectionType sectionType,
		DateTime created,
		string createdBy,
		ICollection<LetterTemplateSectionTemplate>? letterTemplateSectionTemplates = null,
		ICollection<LetterTemplate>? letterTemplates = null,
		ICollection<SnippetTemplate>? snippetTemplates = null,
		Guid? id = null,
		DateTime modified = default,
		string modifiedBy = "")
		: base(id, created, createdBy, modified, modifiedBy)
	{
		LetterTemplateSectionTemplates = letterTemplateSectionTemplates ?? new List<LetterTemplateSectionTemplate>();
		SectionType = sectionType;
		LetterTemplates = letterTemplates ?? new List<LetterTemplate>();
		SnippetTemplates = snippetTemplates ?? new List<SnippetTemplate>();
	}

	public ICollection<LetterTemplateSectionTemplate> LetterTemplateSectionTemplates { get; set; } =
		new List<LetterTemplateSectionTemplate>();

	public SectionType SectionType { get; set; } = new();
	public ICollection<LetterTemplate> LetterTemplates { get; set; } = new List<LetterTemplate>();
	public ICollection<SnippetTemplate> SnippetTemplates { get; set; } = new List<SnippetTemplate>();
}
