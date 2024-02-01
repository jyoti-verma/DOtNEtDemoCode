namespace SmartHospital.Letters.Fhir.Client;

public sealed class FhirRetrivalException<T> : Exception
{
	public FhirRetrivalException(string uri)
		: base($"Object {typeof(T)} could not be retrieved")
	{
		Data.Add(nameof(uri), uri);
	}
}
