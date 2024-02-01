using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Api.Services;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Api.Controllers;

/// <summary>
///     Get letters as html
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.AdministrationOffice + "," + UserRoles.Doctor)]
public sealed class HtmlLettersController
{
	private readonly IConvertableLetter _convertableLetter;

	/// <summary>
	///     Constructor of LettersController.
	/// </summary>
	/// <param name="convertableLetter"></param>
	public HtmlLettersController(
		IConvertableLetter convertableLetter
	)
	{
		_convertableLetter = convertableLetter;
	}

	/// <summary>
	///     Return a letter by its given Id.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(ContentResult), StatusCodes.Status200OK, "html/text")]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<ContentResult> GetById(Guid id, CancellationToken cancellationToken = default)
	{
		return await _convertableLetter.CreateHtmlContentAsync(id, cancellationToken);
	}
}
