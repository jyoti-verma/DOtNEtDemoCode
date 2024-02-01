namespace SmartHospital.Letters.Dtos;

public class Letter
{
	public Guid Id { get; set; }
	public AdmissionTypes AdmissionType { get; set; }
	public string Title { get; set; } = null!;
	public ICollection<Section> Sections { get; set; } = new List<Section>();
	public string PatientFirstname { get; set; } = null!;
	public string PatientLastname { get; set; } = null!;
	public string PatientExternalId { get; set; } = null!;
	public string PatientExternalCaseNumber { get; set; } = null!;
	public string CreatedBy { get; set; } = null!;
	public DateTime Created { get; set; }
	public string ModifiedBy { get; set; } = null!;
	public DateTime Modified { get; set; }
	public LetterStatusTypes Status { get; set; }

	public static Letter FromLetterEntity(Entities.Letter letter)
	{
		return new Letter
		{
			Id = letter.Id,
			AdmissionType = (AdmissionTypes)letter.AdmissionType,
			Title = letter.LetterType.Name,
			Created = letter.Created,
			CreatedBy = letter.CreatedBy,
			Modified = letter.Modified,
			ModifiedBy = letter.ModifiedBy,
			PatientExternalCaseNumber = letter.ExternalCaseNumber,
			PatientExternalId = letter.ExternalPatientId,
			PatientFirstname = letter.FirstName,
			PatientLastname = letter.LastName,
			Sections = FromSection(letter.Sections),
			Status = (LetterStatusTypes)letter.Status
		};
	}

	private static ICollection<Section> FromSection(ICollection<Entities.Section> sections)
	{
		return sections.Select(section => new Section
			{
				Id = section.Id,
				Title = section.Title,
				SortOrder = section.SortOrder,
				SectionTypeId = section.SectionType.Id,
				Snippets = FromSnippets(section.Snippets)
			})
			.ToList();
	}

	private static ICollection<Snippet> FromSnippets(ICollection<Entities.Snippet> snippets)
	{
		return snippets.Select(snippet => new Snippet
			{
				Id = snippet.Id,
				Title = snippet.Title,
				KeyValues = FromValues(snippet.KeyValues),
				SortOrder = snippet.SortOrder
			})
			.ToList();
	}

	private static ICollection<KeyValue> FromValues(IEnumerable<Entities.KeyValue> values)
	{
		return values.Select(keyValue => new KeyValue
			{
				Key = keyValue.Key,
				KeyType = keyValue.KeyType,
				Value = keyValue.Value,
				ValueType = keyValue.ValueType,
				SortOrder = keyValue.SortOrder
			})
			.ToList();
	}
}
