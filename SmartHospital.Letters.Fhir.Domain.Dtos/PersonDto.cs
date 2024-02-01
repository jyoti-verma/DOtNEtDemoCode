namespace SmartHospital.Letters.Fhir.Domain.Dtos;

public abstract class PersonDto : FhirResourceDto
{
	public ICollection<HumanNameDto> HumanNames { get; set; } = new List<HumanNameDto>();
	public ICollection<AddressDto> Addresses { get; set; } = new List<AddressDto>();
}
