using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Context;

namespace SmartHospital.Letters.Api.Controllers;

/// <summary>
///     Controller for Administration
/// </summary>
[Route("[controller]")]
[ApiController]
public sealed class AdminController : ControllerBase
{
	private readonly CheckDbConnection _checkDbConnection;

	/// <summary>
	///     Initializes a new instance of the <see cref="AuthController" /> class.
	/// </summary>
	/// <param name="checkDbConnection"></param>
	public AdminController(CheckDbConnection checkDbConnection)
	{
		_checkDbConnection = checkDbConnection;
	}

	/// <summary>
	///     Returns current loaded assemblies.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route("Info")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public IList<string> GetInfo()
	{
		return new List<string>(
			Assembly.GetExecutingAssembly().GetReferencedAssemblies().Select(
				assemblyName => Assembly.Load(assemblyName).GetName().ToString()
			)) { Assembly.GetExecutingAssembly().GetName().ToString() };
	}

	/// <summary>
	///     Check if service is available.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route("Available")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public IActionResult Available()
	{
		return Ok();
	}

	/// <summary>
	///     Check if FHIR service is available.
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Route("FhirAvailable")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public IActionResult FhirAvailable()
	{
		return Ok();
	}

	/// <summary>
	///     Check if database is available.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpGet]
	[Route("DbAuthAvailable")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> DbAuthAvailable(CancellationToken cancellationToken)
	{
		return await _checkDbConnection.IsDatabaseAvailableAsync(cancellationToken)
			? Ok()
			: StatusCode(StatusCodes.Status503ServiceUnavailable);
	}
}
