namespace SmartHospital.Letters.Entities;

/// <summary>
///     Represents a letter.
/// </summary>
public sealed class Letter : BaseClass
{
	/// <summary>
	///     Used for Serialization/Deserialization
	/// </summary>
	public Letter()
	{
	}

	/// <summary>
	///     Creates a new letter.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="admissionType"></param>
	/// <param name="letterType"></param>
	/// <param name="firstName"></param>
	/// <param name="lastName"></param>
	/// <param name="externalCaseNumber"></param>
	/// <param name="externalPatientId"></param>
	/// <param name="status"></param>
	/// <param name="sections"></param>
	/// <param name="created"></param>
	/// <param name="createdBy"></param>
	/// <param name="modified"></param>
	/// <param name="modifiedBy"></param>
	public Letter(
		Guid id,
		AdmissionTypes admissionType,
		LetterType letterType,
		string firstName,
		string lastName,
		string externalCaseNumber,
		string externalPatientId,
		LetterStatusTypes status,
		ICollection<Section> sections,
		DateTime created,
		string createdBy,
		DateTime modified = default,
		string modifiedBy = ""
	)
		: base(id, created, createdBy, modified, modifiedBy)
	{
		AdmissionType = admissionType;
		LetterType = letterType;
		FirstName = firstName;
		LastName = lastName;
		ExternalCaseNumber = externalCaseNumber;
		ExternalPatientId = externalPatientId;
		Status = status;
		Sections = sections;
	}

	public AdmissionTypes AdmissionType { get; set; }
	public LetterType LetterType { get; set; } = new();
	public string FirstName { get; set; } = "";
	public string LastName { get; set; } = "";
	public string ExternalCaseNumber { get; set; } = "";
	public string ExternalPatientId { get; set; } = "";
	public LetterStatusTypes Status { get; set; }
	public ICollection<Section> Sections { get; set; } = new List<Section>();
}
