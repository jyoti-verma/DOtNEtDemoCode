using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Api.Services;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.UseCases.GetLetterById;

namespace SmartHospital.Letters.Api.Controllers;

/// <summary>
///     Get letters as html
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer", Roles = UserRoles.AdministrationOffice + "," + UserRoles.Doctor)]
public sealed class PdfLettersController
{
	private readonly IConvertableLetter _convertableLetter;

	/// <summary>
	///     Constructor of LettersController.
	/// </summary>
	public PdfLettersController(
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
	[Authorize(Roles = UserRoles.AdministrationOffice + "," + UserRoles.Doctor)]
	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(GetLetterByIdResponse), StatusCodes.Status200OK, "application/pdf")]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = UserRoles.Doctor)]
	public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
	{
		return await _convertableLetter.CreatePdfFileAsync(id, cancellationToken);
	}
}
