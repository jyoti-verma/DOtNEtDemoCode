using SmartHospital.Letters.Core;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Entities;

public sealed class EntityFactory : IEntityFactory
{
	private readonly IDateTimeProvider _dateTimeProvider;

	public EntityFactory(IDateTimeProvider dateTimeProvider)
	{
		_dateTimeProvider = dateTimeProvider;
	}

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
	)
	{
		return new Letter(
			id,
			admissionType,
			letterType,
			firstName,
			lastName,
			externalCaseNumber,
			externalPatientId,
			status,
			sections ?? Array.Empty<Section>(),
			_dateTimeProvider.Now,
			user.UserName!
		);
	}

	public Section CreateSection(
		Guid id,
		string title,
		SectionType sectionType,
		Letter letter,
		ICollection<Snippet> snippets,
		int sortOrder,
		LetterUser user
	)
	{
		return new Section(
			id,
			title,
			sectionType,
			letter,
			snippets,
			sortOrder,
			_dateTimeProvider.Now,
			user.UserName!
		);
	}

	public Snippet CreateSnippet(
		Guid id,
		string title,
		Section section,
		int sortOrder,
		LetterUser user,
		ICollection<KeyValue>? keyValues = null
	)
	{
		return new Snippet(
			id,
			title,
			section,
			sortOrder,
			keyValues ?? Array.Empty<KeyValue>(),
			_dateTimeProvider.Now,
			user.UserName!
		);
	}

	public KeyValue CreateKeyValue(
		Guid id,
		Snippet snippet,
		int sortOrder,
		string value,
		string valueType,
		string key,
		string keyType,
		LetterUser user
	)
	{
		return new KeyValue(
			id,
			snippet,
			sortOrder,
			value,
			valueType,
			key,
			keyType,
			_dateTimeProvider.Now,
			user.UserName!
		);
	}

	public Letter CloneLetter(
		Letter letter,
		string externalCaseNumber,
		LetterUser user
	)
	{
		return new Letter(
			Guid.NewGuid(),
			letter.AdmissionType,
			letter.LetterType,
			letter.FirstName,
			letter.LastName,
			externalCaseNumber,
			letter.ExternalPatientId,
			letter.Status,
			CloneSections(letter.Sections, user).ToList(),
			_dateTimeProvider.Now,
			user.UserName!
		);
	}

	private Section CloneSection(
		Section section,
		LetterUser user
	)
	{
		return new Section(
			Guid.NewGuid(),
			section.Title,
			section.SectionType,
			section.Letter,
			CloneSnippets(section.Snippets, user).ToList(),
			section.SortOrder,
			_dateTimeProvider.Now,
			user.UserName!
		);
	}

	private Snippet CloneSnippet(
		Snippet snippet,
		LetterUser user
	)
	{
		return new Snippet(
			Guid.NewGuid(),
			snippet.Title,
			snippet.Section,
			snippet.SortOrder,
			CloneKeyValues(snippet.KeyValues, user).ToList(),
			_dateTimeProvider.Now,
			user.UserName!);
	}

	private KeyValue CloneKeyValue(
		KeyValue keyValue,
		LetterUser user
	)
	{
		return new KeyValue(
			Guid.NewGuid(),
			keyValue.Snippet,
			keyValue.SortOrder,
			keyValue.Value,
			keyValue.ValueType,
			keyValue.Key,
			keyValue.KeyType,
			_dateTimeProvider.Now,
			user.UserName!
		);
	}

	private ICollection<Section> CloneSections(ICollection<Section> sections, LetterUser user)
	{
		return sections.Select(section => CloneSection(section, user)).ToList();
	}

	private ICollection<Snippet> CloneSnippets(ICollection<Snippet> snippets, LetterUser user)
	{
		return snippets.Select(snippet => CloneSnippet(snippet, user)).ToList();
	}

	private ICollection<KeyValue> CloneKeyValues(ICollection<KeyValue> keyValues, LetterUser user)
	{
		return keyValues.Select(keyValue => CloneKeyValue(keyValue, user)).ToList();
	}
}
