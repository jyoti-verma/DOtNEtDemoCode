using System.Globalization;
using System.Text;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public sealed class SnippedDtosService : ISnippedDtosService
{
	/// <summary>
	///     Create a key value pair.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <param name="sortOrder"></param>
	/// <returns></returns>
	public KeyValue CreateKeyValue(string key, string value, int sortOrder)
	{
		return new KeyValue
		{
			Id = Guid.NewGuid(),
			Key = key,
			KeyType = "string",
			Value = value,
			ValueType = "string",
			SortOrder = sortOrder
		};
	}

	/// <summary>
	///     Create a key value pair with string value.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <param name="valueType"></param>
	/// <param name="sortOrder"></param>
	/// <returns></returns>
	public KeyValue CreateKeyValue(string key, string value, string valueType, int sortOrder)
	{
		return new KeyValue
		{
			Id = Guid.NewGuid(),
			Key = key,
			KeyType = "string",
			Value = value,
			ValueType = valueType,
			SortOrder = sortOrder
		};
	}

	/// <summary>
	///     Create a a Address key value pair.
	/// </summary>
	/// <param name="addressDto"></param>
	/// <returns></returns>
	public IEnumerable<KeyValue> CreateAddressKeyValues(AddressDto addressDto)
	{
		return new AddressSnipped(addressDto, this);
	}

	/// <summary>
	///     Create a unordered html list with list items.
	/// </summary>
	/// <param name="drugIntolerances"></param>
	/// <returns></returns>
	public string CreateUnorderedHtmlList(IEnumerable<string> drugIntolerances)
	{
		var sb = new StringBuilder();
		sb.Append("<ul>");
		foreach (string entry in drugIntolerances)
		{
			string item = $"<li>{entry}</li>";
			sb.Append(item);
		}

		sb.Append("</ul>");

		return sb.ToString();
	}

	/// <summary>
	///     Gets the german "Anrede" depending on the gender.
	/// </summary>
	/// <param name="gender"></param>
	/// <returns></returns>
	public string GetSalutation(int gender)
	{
		return gender switch
		{
			0 => "Herr",
			1 => "Frau",
			_ => "Sehr geehrte Damen und Herren"
		};
	}

	/// <summary>
	///     Returns the full name of a human.
	/// </summary>
	/// <param name="humanNameDto"></param>
	/// <returns></returns>
	public string CreateHumanNameString(HumanNameDto humanNameDto)
	{
		return $"{humanNameDto.Prefix} {humanNameDto.GivenName} {humanNameDto.FamilyName}".Trim();
	}

	/// <summary>
	///     Create a time period string.
	/// </summary>
	/// <param name="periodDto"></param>
	/// <returns></returns>
	public string CreatePeriodString(PeriodDto periodDto)
	{
		string result = periodDto.Start.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
		if (periodDto.End.HasValue)
		{
			result = $"{result}-{periodDto.End.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}";
		}

		return result;
	}


	/// <summary>
	///     Creates a ordered html list with list items.
	/// </summary>
	/// <param name="entries"></param>
	/// <returns></returns>
	public string CreateOrderedHtmlList(IEnumerable<string> entries)
	{
		var sb = new StringBuilder();
		sb.Append("<ol>");
		foreach (string entry in entries)
		{
			string item = $"<li>{entry}</li>";
			sb.Append(item);
		}

		sb.Append("</ol>");

		return sb.ToString();
	}

	/// <summary>
	///     Creates a Salutation for a person.
	/// </summary>
	/// <param name="gender"></param>
	/// <returns></returns>
	public string GetSalutationPerson(int gender)
	{
		return gender switch
		{
			0 => "Sehr geehrter Herr",
			1 => "Sehr geehrte Frau",
			_ => "Sehr geehrte Damen und Herren"
		};
	}

	/// <summary>
	///     Gets the category of a observation.
	/// </summary>
	/// <param name="category"></param>
	/// <returns></returns>
	public string GetCategory(int category)
	{
		return category switch
		{
			0 => "ambulanten",
			1 => "stationären",
			_ => ""
		};
	}
}
