using AutoMapper;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Fhir.Domain;
using SmartHospital.Letters.Fhir.Domain.Dtos;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;

namespace SmartHospital.Fhir.Mock.Api.Controllers;

/// <summary>
///     Controller for managing mock <see cref="DiagnosticReportDto" /> data
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class DiagnosticReportController : ControllerBase
{
	private readonly IGlobalRepo _fhirRepository;
	private readonly IMapper _mapper;

	/// <summary>
	///     Creates a new instance of the <see cref="DiagnosticReportController" /> class.
	/// </summary>
	/// <param name="fhirRepository">The FHIR repository.</param>
	/// <param name="mapper">The mapper.</param>
	public DiagnosticReportController(FhirRepoManager fhirRepository, IMapper mapper)
	{
		_fhirRepository = fhirRepository.GetRepo();
		_mapper = mapper;
	}

	/// <summary>
	///     Returns a diagnostic report list associated with the specified identifier.
	/// </summary>
	/// <param name="identifier">The identifier of the diagnostic report.</param>
	/// <returns>A diagnostic report list.</returns>
	[ProducesResponseType(typeof(IEnumerable<DiagnosticReportDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet]
	public ActionResult<IEnumerable<DiagnosticReportDto>> GetDiagnosticReports(string? identifier = null)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			IEnumerable<DiagnosticReport> list = fhirRepository.DiagnosticReports
			.Where(p =>
				p.Identifier == identifier
				|| string.IsNullOrEmpty(identifier)
			)
			.OrderByDescending(p => p.EffectiveDateTime);

			return Ok(_mapper.Map<IList<DiagnosticReportDto>>(list));
		}
		else if (_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{
			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(identifier))
			{
				searchParams.Query = "DiagnosticReport/" + identifier;
			}
			IEnumerable<DiagnosticReport> diagnosticReports = externalFhirRepo.GetDiagnosticReport(searchParams).Result;

			return Ok(_mapper.Map<IEnumerable<DiagnosticReportDto>>(diagnosticReports));
		}
		return NotFound(new List<DiagnosticReportDto>());

	}

	/// <summary>
	///     Returns a list of diagnostic report associated with the specified identifiers.
	/// </summary>
	/// <param name="patientIdentifier">Identifier of the patient.</param>
	/// <param name="observationIdentifier">Identifier of the observation.</param>
	/// <returns>A list of diagnostic reports.</returns>
	[ProducesResponseType(typeof(IEnumerable<DiagnosticReportDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[HttpGet("ByPatientAndObservation")]
	public ActionResult<IEnumerable<DiagnosticReportDto>> GetDiagnosticReport(
		string? patientIdentifier = null,
		string? observationIdentifier = null
	)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			IEnumerable<DiagnosticReport> entries = fhirRepository.DiagnosticReports
			.Where(
				p =>
					p.Patient.Identifier == patientIdentifier
					&& p.Observation.Identifier == observationIdentifier
			);

			return entries.Any()
				? Ok(_mapper.Map<IEnumerable<DiagnosticReportDto>>(entries))
				: NotFound();
		}
		else if (_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{
			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(patientIdentifier))
			{
				searchParams.Add("patient", patientIdentifier);
			}
			else if (!string.IsNullOrEmpty(observationIdentifier))
			{
				searchParams.Add("observation", observationIdentifier);
			}
			IEnumerable<DiagnosticReport> diagnosticReports = externalFhirRepo.GetDiagnosticReport(searchParams).Result;

			return Ok(_mapper.Map<IEnumerable<DiagnosticReportDto>>(diagnosticReports));
		}
		return NotFound(new List<DiagnosticReport>());
	}
}
