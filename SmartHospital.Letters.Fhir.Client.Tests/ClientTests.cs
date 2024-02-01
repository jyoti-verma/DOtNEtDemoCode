using FluentAssertions;
using Hl7.Fhir.Model;
using Task = System.Threading.Tasks.Task;

namespace SmartHospital.Letters.Fhir.Client.Tests;

public class ClientTests
{
	[Fact]
	public async Task Patients_RetrieveMoreThan5Patients()
	{
		// Arrange
		var client = new Client();

		// Act
		List<Patient> result = await ToListAsync(client.GetFhirAsyncEnumerator<Patient>());

		foreach (Patient patient in result)
		{
			Patient? p = await client.GetFhirAsync<Patient>(patient.ResourceBase);
			if (p != null && p.Name.Any())
			{
			}
		}

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCountGreaterThan(5);
	}

	[Fact]
	public async Task Retrieve()
	{
		var client = new Client();

		List<Organization> result = await ToListAsync(client.GetFhirAsyncEnumerator<Organization>());

		foreach (Organization entry in result)
		{
			Organization? o = await client.GetFhirAsync<Organization>(entry.ResourceBase);
		}
	}


	[Fact]
	public async Task UpdatePatientNames_UpdatesAlwaysTheSameNames()
	{
		// Arrange
		var sut = new Client();
		// Act

		IAsyncEnumerator<Patient> patients = sut.GetFhirAsyncEnumerator<Patient>();
		await patients.MoveNextAsync();
		Patient beforeUpdate = patients.Current;

		IAsyncEnumerator<Patient> patientsAfterUpdate = sut.UpdatePatientNames();

		await patientsAfterUpdate.MoveNextAsync();
		// Assert
		beforeUpdate.Name.Should().BeEquivalentTo(patientsAfterUpdate.Current.Name);
	}

	public async Task<List<T>> ToListAsync<T>(IAsyncEnumerator<T> items) where T : Resource, new()
	{
		var list = new List<T>();
		while (await items.MoveNextAsync())
		{
			list.Add(items.Current);
		}

		return list;
	}
}
