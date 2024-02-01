namespace SmartHospital.Letters.Entities;

public abstract class BaseClass : IBaseClass
{
	protected BaseClass(Guid? id, DateTime created, string createdBy, DateTime modified = default,
		string modifiedBy = "")
	{
		Id = id ?? Guid.NewGuid();
		Created = created;
		CreatedBy = createdBy;
		Modified = modified;
		ModifiedBy = modifiedBy;
	}

	/// <summary>
	///     Used for Serialization/Deserialization
	/// </summary>
	public BaseClass()
	{
	}

	public Guid Id { get; set; } = Guid.NewGuid();
	public DateTime Created { get; set; }
	public string CreatedBy { get; set; } = "";
	public DateTime Modified { get; set; }
	public string ModifiedBy { get; set; } = "";
}
