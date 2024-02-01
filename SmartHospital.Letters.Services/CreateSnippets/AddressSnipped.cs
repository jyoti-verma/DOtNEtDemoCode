using System.Collections;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

/// <summary>
///     Create a a Address IEnumerable key value pair.
/// </summary>
public sealed class AddressSnipped : IEnumerable<KeyValue>
{
	private readonly AddressDto _addressDto;
	private readonly ISnippedDtosService _snippedDtosService;

	public AddressSnipped(AddressDto addressDto, ISnippedDtosService snippedDtosService)
	{
		_addressDto = addressDto;
		_snippedDtosService = snippedDtosService;
	}

	public IEnumerator<KeyValue> GetEnumerator()
	{
		yield return _snippedDtosService.CreateKeyValue("Street", _addressDto.Lines.First(), 1);
		yield return _snippedDtosService.CreateKeyValue("ZipCode", _addressDto.PostalCode, 2);
		yield return _snippedDtosService.CreateKeyValue("City", _addressDto.City, 3);
		yield return _snippedDtosService.CreateKeyValue("Country", _addressDto.Country, 4);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
