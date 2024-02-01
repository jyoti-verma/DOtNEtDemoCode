namespace SmartHospital.Letters.Fhir.Api.Client;

public class FhirApiOptions
{
	public string HostUrl { get; set; } = null!;
	public string Timeout { get; set; } = null!;
	public static string Section { get; set; } = "FhirMockApi";
}
