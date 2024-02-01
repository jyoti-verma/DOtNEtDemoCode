using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.UseCases.Search;

namespace SmartHospital.Letters.Api.Controllers;

/// <summary>
///     Search controller for searching patient(s) and cases
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public sealed class SearchController : ControllerBase
{
	private readonly IMediator _mediator;

	/// <summary>
	///     Creates a new instance of the <see cref="SearchController" /> class.
	/// </summary>
	/// <param name="mediator">Mediator instance</param>
	public SearchController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	///     Search for patient(s) and cases.
	/// </summary>
	/// <remarks>
	///     Michaela Schneider (1679314678)
	///     Cases:<br />137 256 485 <br />or <br />487 164 975
	/// </remarks>
	/// <param name="text"></param>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>A <see cref="SearchResponse" /> object</returns>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<SearchResponse> SearchAsync([Required] string text, CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(new SearchRequest(text), cancellationToken);
	}
}
