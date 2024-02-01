using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Fhir.Domain;
using SmartHospital.Letters.Fhir.Domain.Dtos;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;

namespace SmartHospital.Fhir.Mock.Api.Controllers;

/// <summary>
///     Controller for managing mock <see cref="ObservationDto" /> data
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class ObservationController : ControllerBase
{
	private readonly IGlobalRepo _fhirRepository;
	private readonly IMapper _mapper;

	/// <summary>
	///     Creates a new instance of the <see cref="ObservationController" /> class.
	/// </summary>
	/// <param name="fhirRepository"></param>
	/// <param name="mapper"></param>
	public ObservationController(FhirRepoManager fhirRepository, IMapper mapper)
	{
		_fhirRepository = fhirRepository.GetRepo();
		_mapper = mapper;
	}

	/// <summary>
	///     Returns all observations specified by identifier
	/// </summary>
	/// <param name="identifier">Identifier of an Observation object</param>
	/// <returns></returns>
	[ProducesResponseType(typeof(ObservationDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet("{identifier}")]
	public ActionResult<ObservationDto> GetByIdentifier(
		[Required] string identifier
	)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			IEnumerable<Observation> list = fhirRepository.Observations
			.Where(p =>
				p.Patient.Identifier == identifier
				|| string.IsNullOrEmpty(identifier)
			)
			.OrderByDescending(p => p.EffectiveDateTime);

			return Ok(_mapper.Map<IList<ObservationDto>>(list));
		}
		else if (_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{
			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(identifier))
			{
				searchParams.Query = "Observation/" + identifier;
			}
			IEnumerable<Observation> list = externalFhirRepo.GetObservation(searchParams).Result
				.OrderByDescending(p => p.EffectiveDateTime);

			return Ok(_mapper.Map<IList<ObservationDto>>(list));
		}
		return NotFound(new List<ObservationDto>());
	}

	/// <summary>
	///     Returns the observations. If an patient identifier is given, only the matching row is returned
	/// </summary>
	/// <param name="patientIdentifier">Identifier of a patient</param>
	/// <returns>A list of observations</returns>
	[ProducesResponseType(typeof(IEnumerable<ObservationDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet]
	public ActionResult<IEnumerable<ObservationDto>> GetObservations(string? patientIdentifier = null)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			IEnumerable<Observation> list = fhirRepository.Observations
			.Where(p =>
				p.Patient.Identifier == patientIdentifier
				|| string.IsNullOrEmpty(patientIdentifier)
			)
			.OrderByDescending(p => p.EffectiveDateTime);

			return Ok(_mapper.Map<IList<ObservationDto>>(list));
		}
		else if(_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{
			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(patientIdentifier))
			{
				searchParams.Add("patient", patientIdentifier);
			}
			IEnumerable<Observation> list = externalFhirRepo.GetObservation(searchParams).Result
				.OrderByDescending(p => p.EffectiveDateTime);

			return Ok(_mapper.Map<IList<ObservationDto>>(list));
		}
		return NotFound(new List<ObservationDto>());


	}
}
