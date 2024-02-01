namespace SmartHospital.Letters.Services;

public interface IDbInitializer
{
	/// <summary>
	///     Checks and updates the database if necessary.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task CheckAndUpdateDatabaseAsync(CancellationToken cancellationToken = default);
}
