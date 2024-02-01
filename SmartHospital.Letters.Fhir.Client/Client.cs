using Bogus;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace SmartHospital.Letters.Fhir.Client;

public sealed class Client
{
	public async IAsyncEnumerator<T> GetFhirAsyncEnumerator<T>(CancellationToken cancellationToken = default)
		where T : Resource, new()
	{
		var settings = new FhirClientSettings
		{
			PreferredFormat = ResourceFormat.Json,
			VerifyFhirVersion = true,
			ReturnPreference = ReturnPreference.Minimal
		};

		BaseFhirClient client = new FhirClient("https://fhirserver-development.azurewebsites.net/fhir", settings)
			.WithStrictSerializer();

		Bundle? result = await client.SearchAsync<T>(ct: cancellationToken);

		while (result != null)
		{
			foreach (Bundle.EntryComponent? entry in result.Entry)
			{
				yield return await client.ReadAsync<T>(entry.FullUrl, ct: cancellationToken)
				             ?? throw new FhirRetrivalException<T>(entry.FullUrl);
			}

			result = await client.ContinueAsync(result, ct: cancellationToken);
		}
	}

	public async Task<T?> GetFhirAsync<T>(Uri location, CancellationToken cancellationToken = default)
		where T : Resource, new()
	{
		var settings = new FhirClientSettings
		{
			PreferredFormat = ResourceFormat.Json,
			VerifyFhirVersion = true,
			ReturnPreference = ReturnPreference.Minimal
		};

		BaseFhirClient client = new FhirClient("https://fhirserver-development.azurewebsites.net/fhir", settings)
			.WithStrictSerializer();

		return await client.ReadAsync<T>(location, ct: cancellationToken);
	}

	public async IAsyncEnumerator<Patient> UpdatePatientNames(CancellationToken cancellationToken = default)
	{
		var settings = new FhirClientSettings
		{
			PreferredFormat = ResourceFormat.Json,
			VerifyFhirVersion = true,
			ReturnPreference = ReturnPreference.Minimal
		};

		BaseFhirClient client = new FhirClient("https://fhirserver-development.azurewebsites.net/fhir", settings)
			.WithStrictSerializer();

		Bundle? result = await client.SearchAsync<Patient>(ct: cancellationToken);
		while (result != null)
		{
			foreach (Bundle.EntryComponent? entry in result.Entry)
			{
				Patient currentPatient = await client.ReadAsync<Patient>(entry.FullUrl, ct: cancellationToken)
				                         ?? throw new FhirRetrivalException<Patient>(entry.FullUrl);

				Faker<HumanName>? faker = new Faker<HumanName>("en")
					.UseSeed(currentPatient.Id.GetHashCode())
					.RuleFor(x => x.Given, f => new List<string> { f.Name.FirstName() })
					.RuleFor(x => x.Family, f => f.Name.LastName())
					.RuleFor(x => x.Use, f => HumanName.NameUse.Official);

				currentPatient.Name.Clear();
				currentPatient.Name.Add(
					faker.Generate()
				);
				await client.UpdateAsync(currentPatient, ct: cancellationToken);
				yield return await client.ReadAsync<Patient>(entry.FullUrl, ct: cancellationToken)
				             ?? throw new FhirRetrivalException<Patient>(entry.FullUrl);
			}

			result = await client.ContinueAsync(result, ct: cancellationToken);
		}
	}
}
