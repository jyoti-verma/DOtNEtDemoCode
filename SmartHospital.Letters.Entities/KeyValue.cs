using SmartHospital.Letters.Core;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Entities;

public class KeyValue : BaseClass
{
	/// <summary>
	///     Used for Serialization/Deserialization
	/// </summary>
	public KeyValue()
	{
	}

	public KeyValue(
		Guid id,
		Snippet snippet,
		int sortOrder,
		string value,
		string valueType,
		string key,
		string keyType,
		DateTime created,
		string createdBy,
		DateTime modified = default,
		string modifiedBy = ""
	)
		: base(id, created, createdBy, modified, modifiedBy)
	{
		Id = id;
		Snippet = snippet;
		SortOrder = sortOrder;
		Value = value;
		ValueType = valueType;
		Key = key;
		KeyType = keyType;
	}

	public Snippet Snippet { get; set; } = new();
	public string Value { get; set; } = "";
	public string ValueType { get; set; } = "";
	public string Key { get; set; } = "";
	public string KeyType { get; set; } = "";
	public int SortOrder { get; set; }

	public KeyValue Clone(Guid newGuid, LetterUser user, IDateTimeProvider dateTimeProvider)
	{
		return new KeyValue(newGuid, Snippet, SortOrder, Value, ValueType, Key, KeyType, Created, CreatedBy, Modified,
			ModifiedBy)
		{
			Id = newGuid,
			Value = Value,
			ValueType = ValueType,
			Key = Key,
			KeyType = KeyType,
			SortOrder = SortOrder
		};
	}
}
