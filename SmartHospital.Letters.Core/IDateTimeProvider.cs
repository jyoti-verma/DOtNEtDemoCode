namespace SmartHospital.Letters.Core;

public interface IDateTimeProvider
{
	DateTime Now { get; }
	DateTime MinValue { get; }
}
