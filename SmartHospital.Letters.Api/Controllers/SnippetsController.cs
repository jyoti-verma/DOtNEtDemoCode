using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.UseCases.GetSnippets;

namespace SmartHospital.Letters.Api.Controllers;

/// <summary>
///     Controller for snippet generation
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.AdministrationOffice + "," + UserRoles.Doctor)]
public sealed class SnippetsController : ControllerBase
{
	private readonly IMediator _mediator;

	/// <summary>
	///     Creates a new instance of the <see cref="SnippetsController" /> class.
	/// </summary>
	/// <param name="mediator">Mediator instance</param>
	public SnippetsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	///     Generates a snippet section list for a given section
	/// </summary>
	/// <param name="sectionId">Specific section Guid of a concrete letter</param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>A <see cref="GetSnippetsResponse" /> object</returns>
	[HttpGet("{sectionId:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<GetSnippetsResponse> Get(Guid sectionId, CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(new GetSnippetsRequest(sectionId), cancellationToken);
	}
}
