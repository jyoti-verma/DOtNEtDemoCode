using Microsoft.Extensions.Diagnostics.HealthChecks;
using SmartHospital.Letters.Context;

namespace SmartHospital.Letters.Api;

/// <summary>
///     Health check for the DB.
/// </summary>
public sealed class DbHealthCheck : IHealthCheck
{
	private readonly CheckDbConnection _checkDbConnection;

	/// <summary>
	///     Initializes a new instance of the <see cref="DbHealthCheck" /> class.
	/// </summary>
	/// <param name="checkDbConnection"></param>
	public DbHealthCheck(CheckDbConnection checkDbConnection)
	{
		_checkDbConnection = checkDbConnection;
	}

	/// <summary>
	///     Check the DB connection.
	/// </summary>
	/// <param name="context"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
		CancellationToken cancellationToken = default)
	{
		try
		{
			await _checkDbConnection.IsDatabaseAvailableAsync(cancellationToken);

			return HealthCheckResult.Healthy("The check indicates that the DB is healthy.");
		}

		catch (Exception ex)
		{
			return new HealthCheckResult(
				context.Registration.FailureStatus,
				exception: ex
			);
		}
	}
}
