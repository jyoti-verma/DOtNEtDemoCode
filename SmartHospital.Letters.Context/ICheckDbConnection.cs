namespace SmartHospital.Letters.Context;

public interface ICheckDbConnection
{
	/// <summary>
	///     Check if the database connection can be connected.
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<bool> IsDatabaseAvailableAsync(CancellationToken cancellationToken);
}
