using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartHospital.Letters.Core.Extensions.DependencyInjection;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddLetterRepositories(this IServiceCollection services)
	{
		services.AddCoreServices();

		services.TryAddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
		services.TryAddScoped<ILetterTypeRepository, LetterTypeBaseRepository>();
		services.TryAddScoped<ISectionTypeRepository, SectionTypeBaseRepository>();
		services.TryAddScoped<ILetterTemplateRepository, LetterTemplateBaseRepository>();
		services.TryAddScoped<ISectionTemplateRepository, SectionTemplateBaseRepository>();
		services.TryAddScoped<ILetterTemplateSectionTemplateRepository, LetterTemplateSectionTemplateBaseRepository>();
		services.TryAddScoped<ILetterRepository, LetterBaseRepository>();
		services.TryAddScoped<IRepository<Section>, BaseRepository<Section>>();
		services.TryAddScoped<IRepository<Snippet>, BaseRepository<Snippet>>();
		services.TryAddScoped<IRepository<KeyValue>, BaseRepository<KeyValue>>();
		services.TryAddScoped<ISectionRepository, SectionBaseRepository>();

		return services;
	}
}
