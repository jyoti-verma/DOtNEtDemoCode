using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using RazorLight;

namespace SmartHospital.Letters.Api.Services;

internal sealed class RazorService : IRazorService
{
	private readonly Lazy<RazorLightEngine> _razorEngine = new(BuildRazorEngine);

	public async Task<ContentResult> RenderTemplateAsync(string template, Dtos.Letter model)
	{
		return new ContentResult
		{
			Content = await _razorEngine.Value.CompileRenderAsync(template, model), ContentType = "text/html"
		};
	}

	private static RazorLightEngine BuildRazorEngine()
	{
		return new RazorLightEngineBuilder()
			.UseFileSystemProject(
				Path.Combine(
					Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
					"Views"
				)
			)
			.UseMemoryCachingProvider()
			.Build();
	}
}
