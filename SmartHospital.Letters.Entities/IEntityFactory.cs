using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Entities;

public interface IEntityFactory
{
	public Letter CreateLetter(
		Guid id,
		AdmissionTypes admissionType,
		LetterType letterType,
		string firstName,
		string lastName,
		string externalCaseNumber,
		string externalPatientId,
		LetterStatusTypes status,
		LetterUser user,
		ICollection<Section>? sections = null
	);

	public Letter CloneLetter(
		Letter letter,
		string externalCaseNumber,
		LetterUser user
	);

	public Section CreateSection(
		Guid id,
		string title,
		SectionType sectionType,
		Letter letter,
		ICollection<Snippet> snippets,
		int sortOrder,
		LetterUser user
	);

	public Snippet CreateSnippet(
		Guid id,
		string title,
		Section section,
		int sortOrder,
		LetterUser user,
		ICollection<KeyValue>? keyValues = null);

	public KeyValue CreateKeyValue(
		Guid id,
		Snippet snippet,
		int sortOrder,
		string value,
		string valueType,
		string key,
		string keyType,
		LetterUser user
	);
}
