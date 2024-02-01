namespace SmartHospital.Letters.Core;

public sealed class DateTimeProvider : IDateTimeProvider
{
	public DateTime Now => DateTime.Now;
	public DateTime MinValue => DateTime.MinValue;
}
