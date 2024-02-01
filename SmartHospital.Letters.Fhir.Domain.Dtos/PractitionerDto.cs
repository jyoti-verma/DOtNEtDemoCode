namespace SmartHospital.Letters.Fhir.Domain.Dtos;

public class PractitionerDto : PersonDto
{
	// Normally the Organization is assigned via the PractitionerRole which will not used here for easier implementation 
	public string OrganizationIdentifier { get; set; } = null!;
}
