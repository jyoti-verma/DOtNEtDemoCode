using Microsoft.Extensions.Logging;

namespace SmartHospital.Letters.Context;

public sealed class CheckDbConnection : ICheckDbConnection
{
	private readonly LetterDbContext _dbContext;
	private readonly ILogger<CheckDbConnection> _logger;

	public CheckDbConnection(
		ILogger<CheckDbConnection> logger,
		LetterDbContext dbContext
	)
	{
		_logger = logger;
		_dbContext = dbContext;
	}

	/// <summary>
	///     Check if the database connection can be connected.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<bool> IsDatabaseAvailableAsync(CancellationToken cancellationToken)
	{
		if (await _dbContext.Database.CanConnectAsync(cancellationToken))
		{
			return true;
		}

		_logger.LogWarning("Database is not available");
		return false;
	}
}
