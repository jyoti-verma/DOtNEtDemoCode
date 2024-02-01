using System.Globalization;
using DinkToPdf;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.UseCases.GetLetterById;

namespace SmartHospital.Letters.Api.Services;

/// <summary>
///     Get letters as PDF.
/// </summary>
public sealed class ConvertableLetter : IConvertableLetter
{
	private readonly IMediator _mediator;
	private readonly IRazorService _razorService;

	/// <summary>
	///     Creates an instance of <see cref="ConvertableLetter" />.
	/// </summary>
	/// <param name="mediator"></param>
	/// <param name="razorService"></param>
	public ConvertableLetter(IMediator mediator, IRazorService razorService)
	{
		_mediator = mediator;
		_razorService = razorService;
	}


	/// <summary>
	///     Creates a PDF file from a letter.
	/// </summary>
	/// <param name="letterId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	/// <exception cref="LetterNotFoundException"></exception>
	public async Task<FileContentResult> CreatePdfFileAsync(Guid letterId,
		CancellationToken cancellationToken = default)
	{
		Dtos.Letter? letter = (await _mediator.Send(new GetLetterByIdRequest(letterId), cancellationToken)).Value;

		if (letter is null)
		{
			throw new LetterNotFoundException(letterId);
		}

		ContentResult contentResult = await _razorService.RenderTemplateAsync(
			"LetterAsHtml/Letter",
			letter
		);

		var doc = new HtmlToPdfDocument
		{
			GlobalSettings =
			{
				ColorMode = ColorMode.Color, Orientation = Orientation.Portrait, PaperSize = PaperKind.A4
			},
			Objects =
			{
				new ObjectSettings
				{
					PagesCount = true,
					HtmlContent = contentResult.Content,
					WebSettings = { DefaultEncoding = "utf-8" },
					HeaderSettings =
					{
						FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812
					}
				}
			}
		};
		var fileResponse = new FileContentResult(
			new SynchronizedConverter(new PdfTools()).Convert(doc),
			"application/pdf"
		);


		fileResponse.FileDownloadName =
			$"letter_{letter.Id}_{letter.Modified.ToString("yyyy-MM-dd_HHmmss", CultureInfo.InvariantCulture)}.pdf";

		return fileResponse;
	}

	/// <summary>
	///     Creates a HTML from a letter.
	/// </summary>
	/// <param name="letterId"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<ContentResult> CreateHtmlContentAsync(Guid letterId,
		CancellationToken cancellationToken = default)
	{
		return await _razorService.RenderTemplateAsync(
			"LetterAsHtml/Letter",
			(await _mediator.Send(new GetLetterByIdRequest(letterId), cancellationToken)).Value!
		);
	}
}
