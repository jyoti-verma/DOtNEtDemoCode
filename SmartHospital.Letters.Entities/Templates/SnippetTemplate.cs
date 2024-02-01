using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SmartHospital.Letters.Entities.Templates;

public class SnippetTemplate : BaseClass
{
	public SnippetTemplate()
	{
	}

	public SnippetTemplate(Guid id, string name, ICollection<KeyValue> keyValues, int sortOrder)
	{
		Id = id;
		Name = name;
		KeyValues = keyValues;
		SortOrder = sortOrder;
	}

	public SectionTemplate SectionTemplate { get; set; } = new();
	public string Name { get; set; } = "";
	public string DataType { get; set; } = "string";
	public int SortOrder { get; set; }

	[NotMapped] public ICollection<KeyValue> KeyValues { get; set; } = new List<KeyValue>();

	[Column("KeyValues")]
	public string KeyValuesJson
	{
		get => JsonConvert.SerializeObject(KeyValues);
		set => KeyValues = JsonConvert.DeserializeObject<ICollection<KeyValue>>(value) ?? new List<KeyValue>();
	}
}
