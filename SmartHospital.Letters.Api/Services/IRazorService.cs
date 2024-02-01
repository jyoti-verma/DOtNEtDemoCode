using Microsoft.AspNetCore.Mvc;

namespace SmartHospital.Letters.Api.Services;

/// <summary>
///     Service to render razor templates.
/// </summary>
public interface IRazorService
{
	/// <summary>
	///     Render a template with a given model.
	/// </summary>
	/// <param name="template"></param>
	/// <param name="model"></param>
	/// <returns></returns>
	Task<ContentResult> RenderTemplateAsync(string template, Dtos.Letter model);
}
