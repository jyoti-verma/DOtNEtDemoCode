namespace SmartHospital.Letters.Fhir.Domain;

public class Enums
{
	public enum Categories
	{
		Ambulance,
		Stationary,
		Unknown
	}

	public enum ClinicalStatus
	{
		Active,
		Recurrence,
		Relapse,
		Inactive,
		Resolved,
		Unknown
	}

	public enum Genders
	{
		Male,
		Female,
		Other,
		Unknown
	}

	public enum SpecimenMethod
	{
		Unknown,
		Biopsy,
		Resection,
		LiquidBiopsy,
		Cytology,
	}

	public enum StatusCodes
	{
		Planned,
		Active,
		Finished,
		Cancelled,
		Unknown
	}

	public enum VerificationStatus
	{
		Unconfirmed,
		Confirmed,
		Unknown
	}

	public enum FHIRCallType
	{
		Manual,
		ExternalFhirServer,
		Unknown
	}
	public enum FHIRCallMode
	{
		Patient,
		Observation,
	}
}
