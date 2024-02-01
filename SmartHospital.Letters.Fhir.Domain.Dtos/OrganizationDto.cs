namespace SmartHospital.Letters.Fhir.Domain.Dtos;

public class OrganizationDto : FhirResourceDto
{
	public string Name { get; set; } = "";
	public CodingDto Type { get; set; } = null!;
	public AddressDto Address { get; set; } = null!;
	public string PartOfOrganizationIdentifier { get; set; } = null!;
}
