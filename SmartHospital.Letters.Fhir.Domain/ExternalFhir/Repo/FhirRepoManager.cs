
using Microsoft.Extensions.Configuration;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;
public class FhirRepoManager
{
	private readonly IExternalFhirRepo _externalFhirRepo;
	private readonly IFhirRepository _fhirRepository;
	private readonly bool useExternalRepo;

	public FhirRepoManager(IExternalFhirRepo externalFhirRepo, IFhirRepository fhirRepository,IConfiguration configuration)
	{
		_externalFhirRepo = externalFhirRepo;
		_fhirRepository = fhirRepository;
		string callMode = FhirConfiguration.FhirCallingMode.ToString();
		if (callMode == Enums.FHIRCallType.ExternalFhirServer.ToString())
		{
			useExternalRepo = true;
		}
	}

	public IGlobalRepo GetRepo()
	{
		return useExternalRepo ? _externalFhirRepo : _fhirRepository;
	}
}
