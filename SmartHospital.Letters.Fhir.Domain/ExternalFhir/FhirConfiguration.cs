using Microsoft.Extensions.Configuration;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir;
public static class FhirConfiguration
{
	public static IConfiguration? Config { get; set; }
	public static string FhirCallingMode
	{
		get
		{
			return Config["GlobalSettings:CallMode"];
		}
	}
	public static string FhirExternalURL
	{
		get
		{
			return Config["GlobalSettings:CallUrl"];
		}
	}

}
