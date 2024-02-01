using AutoMapper;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Fhir.Domain;
using SmartHospital.Letters.Fhir.Domain.Dtos;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;
using Observation = SmartHospital.Letters.Fhir.Domain.Observation;
using Patient = SmartHospital.Letters.Fhir.Domain.Patient;

namespace SmartHospital.Fhir.Mock.Api.Controllers;

/// <summary>
///     Controller for managing mock <see cref="PatientDto" /> data
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class PatientController : ControllerBase
{
	private readonly IGlobalRepo _fhirRepository;
	private readonly IMapper _mapper;

	/// <summary>
	///     Creates a new instance of the <see cref="PatientController" /> class.
	/// </summary>
	/// <param name="fhirRepository">The FHIR repository.</param>
	/// <param name="mapper">The mapper.</param>
	public PatientController(FhirRepoManager fhirRepository, IMapper mapper)
	{
		_fhirRepository = fhirRepository.GetRepo();
		_mapper = mapper;
	}

	/// <summary>
	///     Get all patients which name contains searchName or has observation with identifier.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="observationIdentifier"></param>
	/// <returns></returns>
	[ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet]
	public ActionResult<IEnumerable<PatientDto>> GetPatients(string? name = null, string? observationIdentifier = null)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(observationIdentifier))
			{
				return Ok(_mapper.Map<IEnumerable<PatientDto>>(fhirRepository.Patients));
			}
			if (name is not null)
			{
				return Ok(_mapper.Map<IEnumerable<PatientDto>>(
						fhirRepository
							.Patients
							.Where(f =>
								f.HumanNames.Any(s =>
									s.FamilyName.Contains(name, StringComparison.InvariantCultureIgnoreCase)
									|| s.GivenName.Contains(name, StringComparison.InvariantCultureIgnoreCase)
								)
							)
					)
				);
			}
			Observation? entry = fhirRepository.Observations
				.SingleOrDefault(p => p.Identifier == observationIdentifier);

			return entry is not null
				? Ok(new List<PatientDto> { _mapper.Map<PatientDto>(entry) })
				: NotFound(new List<PatientDto>());
		}
		else if (_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{
			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(observationIdentifier))
			{
				searchParams.Add("observation", observationIdentifier);
			}
			else if (!string.IsNullOrEmpty(name))
			{
				searchParams.Add("name", name);
			}
			IEnumerable<Patient> patients = externalFhirRepo.GetPatient2(searchParams).Result;

			return Ok(_mapper.Map<IEnumerable<PatientDto>>(patients));
		}
		return NotFound(new List<PatientDto>());
	}

	/// <summary>
	///     Get all patients which has identifier are equals identifier.
	/// </summary>
	/// <param name="identifier"></param>
	/// <returns></returns>
	[ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet("{identifier}")]
	public ActionResult<PatientDto> GetByIdentifier(string identifier)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			Patient? entry = fhirRepository
		.Patients
		.SingleOrDefault(f => f.Identifier == identifier);

			return entry is not null
				? Ok(_mapper.Map<PatientDto>(entry))
				: NotFound();
		}
		return NotFound();

	}
}
