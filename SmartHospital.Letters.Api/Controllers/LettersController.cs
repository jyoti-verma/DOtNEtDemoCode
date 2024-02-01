using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.UseCases.CreateLetter;
using SmartHospital.Letters.UseCases.CreateLetterFromExisting;
using SmartHospital.Letters.UseCases.GetLetterById;
using SmartHospital.Letters.UseCases.GetLetters;
using SmartHospital.Letters.UseCases.UpdateLetter;

namespace SmartHospital.Letters.Api.Controllers;

/// <summary>
///     Manager Letters
/// </summary>
[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public sealed class LettersController : ControllerBase
{
	private readonly IMediator _mediator;
	private readonly UserManager<LetterUser> _userManager;

	/// <summary>
	///     Constructor of LettersController.
	/// </summary>
	/// <param name="mediator"></param>
	/// <param name="userManager"></param>
	public LettersController(
		IMediator mediator,
		UserManager<LetterUser> userManager)
	{
		_mediator = mediator;
		_userManager = userManager;
	}

	/// <summary>
	///     Creates a new letter.
	/// </summary>
	/// <remarks>
	///     Type: Therapie<br />
	///     AdmissionType: Ambulance<br />
	///     1679314678(Michaela Schneider) for Case 137256485
	/// </remarks>
	/// <param name="admissionType"></param>
	/// <param name="externalPatientId">
	///     <example>1679314678</example>
	/// </param>
	/// <param name="externalCaseNumber">
	///     <example>137256485</example>
	/// </param>
	/// <param name="cancellationToken"></param>
	/// <param name="letterTypeName"></param>
	/// <returns></returns>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = UserRoles.Doctor)]
	public async Task<CreateLetterResponse> PostLetterAsync(
		[Required] string letterTypeName,
		[Required] AdmissionTypes admissionType,
		[Required] string externalPatientId,
		[Required] string externalCaseNumber,
		CancellationToken cancellationToken = default
	)
	{
		return await _mediator.Send(
			new CreateLetterRequest(
				letterTypeName,
				(Entities.AdmissionTypes)admissionType,
				externalPatientId,
				externalCaseNumber,
				await _userManager.FindByIdAsync(_userManager.GetUserId(User)!)
				?? throw new InvalidOperationException("User Could not be loaded")
			),
			cancellationToken
		);
	}

	/// <summary>
	///     Copy an existing letter by its given Id.
	/// </summary>
	/// <param name="existingId"></param>
	/// <param name="externalCaseNumber">The externalCaseNumber</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost("{existingId:guid}/{externalCaseNumber}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = UserRoles.Doctor)]
	public async Task<CreateLetterFromExistingResponse> CreateLetterFromExistingAsync(
		Guid existingId,
		string externalCaseNumber,
		CancellationToken cancellationToken = default
	)
	{
		return await _mediator.Send(
			new CreateLetterFromExistingRequest(
				existingId,
				externalCaseNumber,
				await _userManager.FindByIdAsync(_userManager.GetUserId(User)!)
				?? throw new InvalidOperationException("User Could not be loaded")
			),
			cancellationToken
		);
	}

	/// <summary>
	///     Returns a list of letters.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = UserRoles.AdministrationOffice + "," + UserRoles.Doctor)]
	public async Task<GetLettersResponse> Get(CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(new GetLettersRequest(), cancellationToken);
	}

	/// <summary>
	///     Return a letter by its given Id.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = UserRoles.AdministrationOffice + "," + UserRoles.Doctor)]
	public async Task<GetLetterByIdResponse> GetById(Guid id, CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(new GetLetterByIdRequest(id), cancellationToken);
	}

	/// <summary>
	///     Updates an existing letter.
	/// </summary>
	/// <param name="letter"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = UserRoles.Doctor)]
	public async Task<UpdateLetterResponse> Put(Dtos.Letter letter, CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(
			new UpdateLetterRequest(
				letter,
				(await _userManager.FindByIdAsync(_userManager.GetUserId(User)!))!
			),
			cancellationToken
		);
	}
}
