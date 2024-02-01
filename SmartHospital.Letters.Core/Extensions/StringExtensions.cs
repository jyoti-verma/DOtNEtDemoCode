namespace SmartHospital.Letters.Core.Extensions;

public static class StringExtensions
{
	public static string ToCamelCase(this string value)
	{
		return value.Length < 2
			? value
			: string.Concat(value[0].ToString().ToLower(), value.AsSpan(1));
	}

	public static string RemoveWhitespace(this string input)
	{
		return new string(input.ToCharArray()
			.Where(c => !char.IsWhiteSpace(c))
			.ToArray());
	}
}
