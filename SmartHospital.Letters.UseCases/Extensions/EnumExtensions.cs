using System.Globalization;

namespace SmartHospital.Letters.UseCases.Extensions;

public static class EnumExtensions
{
	public static int ToInt(this Enum value)
	{
		return Convert.ToInt32(value, CultureInfo.InvariantCulture);
	}
}
