using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.UseCases;

public class EntitiesHelper
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly DbContext _dbContext;

	public EntitiesHelper(IDateTimeProvider dateTimeProvider, DbContext dbContext)
	{
		_dateTimeProvider = dateTimeProvider;
		_dbContext = dbContext;
	}

	public void UpdateModifiedProperties(string? username = null)
	{
		DateTime currentDate = _dateTimeProvider.Now;
		string currentUser = username
		                     ?? Environment.UserDomainName + "\\" + Environment.UserName;

		foreach (EntityEntry entry in _dbContext.ChangeTracker.Entries())
		{
			if (entry is { Entity: IBaseClass, State: EntityState.Added })
			{
				entry.Property(nameof(IBaseClass.Created)).CurrentValue = currentDate;
				entry.Property(nameof(IBaseClass.Modified)).CurrentValue = currentDate;
			}

			if (entry is { Entity: IBaseClass, State: EntityState.Modified })
			{
				entry.Property(nameof(IBaseClass.Modified)).CurrentValue = currentDate;
			}
		}
	}
}
