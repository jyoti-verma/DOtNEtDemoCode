namespace SmartHospital.Letters.Services.DefaultValues;

public interface IDefaultValues
{
	public Task CreateAsync(CancellationToken cancellationToken = default);
}
