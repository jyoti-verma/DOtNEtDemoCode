using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Entities.Templates;

namespace SmartHospital.Letters.Context;

public class LetterDbContext : IdentityDbContext<
	LetterUser,
	IdentityRole,
	string,
	IdentityUserClaim<string>,
	IdentityUserRole<string>,
	IdentityUserLogin<string>,
	IdentityRoleClaim<string>,
	IdentityUserToken<string>
>
{
	public LetterDbContext(DbContextOptions<LetterDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<LetterUser> LetterUsers { get; set; } = default!;
	public virtual DbSet<LetterType> LetterTypes { get; set; } = default!;
	public virtual DbSet<SectionType> SectionTypes { get; set; } = default!;
	public virtual DbSet<LetterTemplate> LetterTemplates { get; set; } = default!;
	public virtual DbSet<SectionTemplate> SectionTemplates { get; set; } = default!;
	public virtual DbSet<SnippetTemplate> SnippetTemplates { get; set; } = default!;
	public virtual DbSet<Entities.Letter> Letters { get; set; } = default!;
	public virtual DbSet<Section> Sections { get; set; } = default!;
	public virtual DbSet<Snippet> Snippets { get; set; } = default!;
	public virtual DbSet<KeyValue> Values { get; set; } = default!;

	public virtual DbSet<LetterTemplateSectionTemplate> LetterTemplatesSectionTemplates { get; set; } = default!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<BaseClass>().UseTpcMappingStrategy();

		builder.ApplyConfigurationsFromAssembly(
			Assembly.GetExecutingAssembly(),
			t => t.GetInterfaces().Any(i =>
				i.IsGenericType &&
				i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
				typeof(BaseClass).IsAssignableFrom(i.GenericTypeArguments[0]))
		);
	}
}
