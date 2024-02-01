using AutoMapper;
using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Fhir.Domain;
using SmartHospital.Letters.Fhir.Domain.Dtos;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;

namespace SmartHospital.Fhir.Mock.Api.Controllers;

/// <summary>
///     Controller for managing mock <see cref="OrganizationDto" /> data
/// </summary>
[ApiController]
[Route("[controller]")]
public sealed class OrganizationController : ControllerBase
{
	private readonly IGlobalRepo _fhirRepository;
	private readonly IMapper _mapper;

	/// <summary>
	///     Creates a new instance of the <see cref="OrganizationController" /> class.
	/// </summary>
	/// <param name="fhirRepository">The FHIR repository.</param>
	/// <param name="mapper">The mapper.</param>
	public OrganizationController(FhirRepoManager fhirRepository, IMapper mapper)
	{
		_fhirRepository = fhirRepository.GetRepo();
		_mapper = mapper;
	}

	/// <summary>
	///     Returns the organizations. If an identifier is given, only the matching row is returned
	/// </summary>
	/// <param name="identifier"></param>
	/// <returns>A list of organizations</returns>
	[ProducesResponseType(typeof(IEnumerable<OrganizationDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpGet]
	public ActionResult<IEnumerable<OrganizationDto>> GetOrganizations(string? identifier = null)
	{
		if (_fhirRepository is IFhirRepository fhirRepository)
		{
			IEnumerable<Organization> list = fhirRepository.Organizations
			.Where(p =>
				p.Identifier == identifier
				|| string.IsNullOrEmpty(identifier)
			)
			.OrderByDescending(p => p.Identifier);
			return Ok(_mapper.Map<IEnumerable<OrganizationDto>>(list));

		}
		else if (_fhirRepository is IExternalFhirRepo externalFhirRepo)
		{

			SearchParams searchParams = new SearchParams();
			if (!string.IsNullOrEmpty(identifier))
			{
				searchParams.Query = "Organization/" + identifier;
			}
			IEnumerable<Organization> organizations = externalFhirRepo.GetOrganization(searchParams).Result;

			return Ok(_mapper.Map<IEnumerable<OrganizationDto>>(organizations));
		}
		return NotFound(new List<OrganizationDto>());
	}
}
