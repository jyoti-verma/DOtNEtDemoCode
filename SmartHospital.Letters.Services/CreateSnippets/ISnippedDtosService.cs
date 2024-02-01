using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public interface ISnippedDtosService
{
	/// <summary>
	///     Create a key value pair.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <param name="sortOrder"></param>
	/// <returns></returns>
	KeyValue CreateKeyValue(string key, string value, int sortOrder);

	/// <summary>
	///     Create a key value pair with string value.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <param name="valueType"></param>
	/// <param name="sortOrder"></param>
	/// <returns></returns>
	KeyValue CreateKeyValue(string key, string value, string valueType, int sortOrder);

	/// <summary>
	///     Create a a Address key value pair.
	/// </summary>
	/// <param name="addressDto"></param>
	/// <returns></returns>
	IEnumerable<KeyValue> CreateAddressKeyValues(AddressDto addressDto);

	/// <summary>
	///     Create a unordered html list with list items.
	/// </summary>
	/// <param name="drugIntolerances"></param>
	/// <returns></returns>
	string CreateUnorderedHtmlList(IEnumerable<string> drugIntolerances);

	/// <summary>
	///     Gets the german "Anrede" depending on the gender.
	/// </summary>
	/// <param name="gender"></param>
	/// <returns></returns>
	string GetSalutation(int gender);

	/// <summary>
	///     Returns the full name of a human.
	/// </summary>
	/// <param name="humanNameDto"></param>
	/// <returns></returns>
	string CreateHumanNameString(HumanNameDto humanNameDto);

	/// <summary>
	///     Create a time period string.
	/// </summary>
	/// <param name="periodDto"></param>
	/// <returns></returns>
	string CreatePeriodString(PeriodDto periodDto);

	/// <summary>
	///     Creates a ordered html list with list items.
	/// </summary>
	/// <param name="entries"></param>
	/// <returns></returns>
	string CreateOrderedHtmlList(IEnumerable<string> entries);

	/// <summary>
	///     Creates a Salutation for a person.
	/// </summary>
	/// <param name="gender"></param>
	/// <returns></returns>
	string GetSalutationPerson(int gender);

	/// <summary>
	///     Gets the category of a observation.
	/// </summary>
	/// <param name="category"></param>
	/// <returns></returns>
	string GetCategory(int category);
}
