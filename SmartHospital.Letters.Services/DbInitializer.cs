using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Repositories;
using SmartHospital.Letters.Services.DefaultValues;

namespace SmartHospital.Letters.Services;

public sealed class DbInitializer : IDbInitializer
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly LetterDbContext _dbContext;
	private readonly IEntityFactory _entityFactory;
	private readonly ILetterTemplateRepository _letterTemplateRepository;
	private readonly ILetterTemplateSectionTemplateRepository _letterTemplateSectionTemplateRepository;
	private readonly ILetterTypeRepository _letterTypeRepository;
	private readonly ILoggerFactory _loggerFactory;

	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly ISectionTemplateRepository _sectionTemplateRepository;
	private readonly ISectionTypeRepository _sectionTypeRepository;
	private readonly UserManager<LetterUser> _userManager;

	public DbInitializer(
		UserManager<LetterUser> userManager,
		RoleManager<IdentityRole> roleManager,
		ILetterTypeRepository letterTypeRepository,
		ISectionTypeRepository sectionTypeRepository,
		ILetterTemplateRepository letterTemplateRepository,
		ISectionTemplateRepository sectionTemplateRepository,
		ILetterTemplateSectionTemplateRepository letterTemplateSectionTemplateRepository,
		LetterDbContext dbContext,
		IEntityFactory entityFactory,
		ILoggerFactory loggerFactory,
		IDateTimeProvider dateTimeProvider
	)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_letterTypeRepository = letterTypeRepository;
		_sectionTypeRepository = sectionTypeRepository;
		_letterTemplateRepository = letterTemplateRepository;
		_sectionTemplateRepository = sectionTemplateRepository;
		_letterTemplateSectionTemplateRepository = letterTemplateSectionTemplateRepository;
		_dbContext = dbContext;
		_entityFactory = entityFactory;
		_loggerFactory = loggerFactory;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task CheckAndUpdateDatabaseAsync(CancellationToken cancellationToken = default)
	{
		await _dbContext.Database.MigrateAsync(cancellationToken);
		await new DefaultUsers(_roleManager, _userManager).CreateAsync(cancellationToken);

		await new DefaultLetterTypes(_letterTypeRepository, _dateTimeProvider).CreateAsync(cancellationToken);
		await new DefaultSectionTypes(
			_sectionTypeRepository,
			_loggerFactory.CreateLogger<DefaultSectionTypes>(),
			_dateTimeProvider
		).CreateAsync(cancellationToken);

		await new DefaultSectionTemplates(
			_sectionTypeRepository,
			_sectionTemplateRepository,
			_entityFactory,
			_dateTimeProvider
		).CreateAsync(cancellationToken);

		await new DefaultLettersTemplates(
			_letterTypeRepository,
			_sectionTemplateRepository,
			_letterTemplateSectionTemplateRepository,
			_letterTemplateRepository,
			_dateTimeProvider
		).CreateAsync(cancellationToken);
	}
}
