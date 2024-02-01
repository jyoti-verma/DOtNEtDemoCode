using AutoMapper;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Fhir.Domain;
using SmartHospital.Letters.Fhir.Domain.Dtos;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;

namespace SmartHospital.Fhir.Mock.Api.Controllers;

/// <summary>
///     Controller for managing mock <see cref="ConditionDto" /> data
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class ConditionController : ControllerBase
{
	private readonly IGlobalRepo _fhirRepository;
	private readonly IMapper _mapper;

	/// <summary>
	///     Creates a new instance of the <see cref="ConditionController" /> class.
	/// </summary>
	/// <param name="fhirRepository">The FHIR repository.</param>
	/// <param name="mapper">The mapper.</param>
	public ConditionController(FhirRepoManager fhirRepository, IMapper mapper)
	{
		_fhirRepository = fhirRepository.GetRepo();
		_mapper = mapper;
	}

	/// <summary>
	///     Returns conditions associated with the specified identifier.
	/// </summary>
	/// <param name="identifier">The identifier of the condition.</param>
	/// <returns>A list of conditions.</returns>
	[ProducesResponseType(typeof(IEnumerable<ConditionDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet]
	public ActionResult<IEnumerable<ConditionDto>> GetConditions(string? identifier = null)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			IEnumerable<Condition> list = fhirRepository.Conditions
			.Where(p =>
				p.Identifier == identifier
				|| string.IsNullOrEmpty(identifier)
			);

			return Ok(_mapper.Map<IEnumerable<ConditionDto>>(list));
		}
		else if (_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{
			
			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(identifier))
			{
				searchParams.Query = "Condition/" + identifier;
			}
			//var resourceType = "Condition";
			//IEnumerable<Condition> conditions = externalFhirRepo.GetCondition(searchParams, resourceType);
			IEnumerable<Condition> conditions = externalFhirRepo.GetCondition(searchParams).Result;
			


			return Ok(_mapper.Map<IEnumerable<ConditionDto>>(conditions));
		}
		return NotFound(new List<ConditionDto>());

	}

	/// <summary>
	///     Returns conditions associated with the specified patient identifier.
	/// </summary>
	/// <param name="identifier">The identifier of the patient.</param>
	/// <returns>A list of conditions associated with the patient.</returns>
	[ProducesResponseType(typeof(IEnumerable<ConditionDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet("Patient/{identifier}")]
	public ActionResult<IEnumerable<ConditionDto>> GetByPatientIdentifier(string identifier)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			return Ok(_mapper.Map<IList<ConditionDto>>(
				fhirRepository
					.Conditions
					.Where(p =>
						p.Patient.Identifier == identifier)
					.OrderBy(p => p.RecordedDate.Start)
			)
		);
		}
		else if (_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{
			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(identifier))
			{
				searchParams.Add("patient",identifier);
			}
			//var resourceType = "Condition";
			//IEnumerable<Condition> conditions = externalFhirRepo.GetCondition(searchParams, resourceType);
			IEnumerable<Condition> conditions = externalFhirRepo.GetCondition(searchParams).Result;

			return Ok(_mapper.Map<IEnumerable<ConditionDto>>(conditions));
		}
		return NotFound(new List<ConditionDto>());
	}

	/// <summary>
	///     Returns conditions associated with the specified observation identifier.
	/// </summary>
	/// <param name="identifier">The identifier of the observation.</param>
	/// <returns>A list of conditions associated with the observation.</returns>
	[ProducesResponseType(typeof(IEnumerable<ConditionDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet("Observation/{identifier}")]
	public ActionResult<ConditionDto> GetByObservationIdentifier(string identifier)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			return Ok(_mapper.Map<IList<ConditionDto>>(
				fhirRepository
					.Conditions
					.Where(p =>
						p.Observation.Identifier == identifier)
					.OrderBy(p => p.RecordedDate)
			)
		);
		}
		else if (_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{
			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(identifier))
			{
				if (!string.IsNullOrEmpty(identifier))
				{
					searchParams.Add("evidence", identifier);
				}
			}
			//var resourceType = "Condition";
			//IEnumerable<Condition> conditions = externalFhirRepo.GetCondition(searchParams, resourceType);
			IEnumerable<Condition> conditions = externalFhirRepo.GetCondition(searchParams).Result;

			return Ok(_mapper.Map<IEnumerable<ConditionDto>>(conditions));
		}
		return NotFound(new List<ConditionDto>());
	}
}
