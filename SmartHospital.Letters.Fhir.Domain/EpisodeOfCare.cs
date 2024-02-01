namespace SmartHospital.Letters.Fhir.Domain;

public class EpisodeOfCare : FhirResource
{
	public Patient Patient { get; set; } = null!;
	public Enums.StatusCodes Status { get; set; }
	public Period Period { get; set; } = null!;
}
