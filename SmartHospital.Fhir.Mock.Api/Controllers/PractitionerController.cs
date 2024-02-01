using AutoMapper;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Fhir.Domain;
using SmartHospital.Letters.Fhir.Domain.Dtos;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;

namespace SmartHospital.Fhir.Mock.Api.Controllers;

/// <summary>
///     Controller for managing mock <see cref="PractitionerDto" /> data
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class PractitionerController : ControllerBase
{
	private readonly IGlobalRepo _fhirRepository;
	private readonly IMapper _mapper;

	/// <summary>
	///     Creates a new instance of the <see cref="PatientController" /> class.
	/// </summary>
	/// <param name="fhirRepository"></param>
	/// <param name="mapper"></param>
	public PractitionerController(FhirRepoManager fhirRepository, IMapper mapper)
	{
		_fhirRepository = fhirRepository.GetRepo();
		_mapper = mapper;
	}

	/// <summary>
	///     Get all patients which has identifier are equals identifier.
	/// </summary>
	/// <param name="identifier"></param>
	/// <returns></returns>
	[ProducesResponseType(typeof(IEnumerable<PractitionerDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet]
	public ActionResult<IEnumerable<PractitionerDto>> GetPractitioners(string? identifier = null)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			IEnumerable<Practitioner> list = fhirRepository.Practitioners
				.Where(p =>
					p.Identifier == identifier
					|| string.IsNullOrEmpty(identifier)
				);

			return Ok(_mapper.Map<IEnumerable<PractitionerDto>>(list));
		}
		else if (_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{
			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(identifier))
			{
				searchParams.Query = "Practitioner/" + identifier;
			}
			IEnumerable<Practitioner> practitioners = externalFhirRepo.GetPractitioner(searchParams).Result;
			return Ok(_mapper.Map<IList<PractitionerDto>>(practitioners));
		}
		return NotFound(new List<PractitionerDto>());
	}
}
