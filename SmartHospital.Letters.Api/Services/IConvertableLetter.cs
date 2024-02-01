using Microsoft.AspNetCore.Mvc;

namespace SmartHospital.Letters.Api.Services;

/// <summary>
///     Service to convert a letter to another format.
/// </summary>
public interface IConvertableLetter
{
	/// <summary>
	///     Creates a PDF file from a letter by rendering it as HTML and converting it to PDF format.
	/// </summary>
	/// <param name="letterId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<FileContentResult> CreatePdfFileAsync(Guid letterId, CancellationToken cancellationToken = default);


	/// <summary>
	///     Returns the HTML content of a letter.
	/// </summary>
	/// <param name="letterId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<ContentResult> CreateHtmlContentAsync(Guid letterId, CancellationToken cancellationToken = default);
}
