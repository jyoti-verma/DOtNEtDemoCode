using System.Collections;
using System.Globalization;

namespace SmartHospital.Letters.Fhir.Domain;

public sealed class MockPractitionersCollection : IEnumerable<Practitioner>
{
	private static readonly List<Organization> Organizations = new MockOrganizationsCollection().ToList();
	private readonly List<Practitioner> _practitioners;

	public MockPractitionersCollection()
	{
		_practitioners = new List<Practitioner>();
		Practitioner pr1 = CreatePractitioner1(
			Organizations
				.Single(p => p.Name == "Universitätsklinikum Essen Innere Klinik"));
		Practitioner pr2 = CreatePractitioner2(
			Organizations
				.Single(p => p.Name == "Radiologie Dr. Ulrike Musterfrau"));
		Practitioner pr3 = CreatePractitioner3(
			Organizations
				.Single(p => p.Name == "Musterpraxis Dr. Alfred Mustermann"));

		_practitioners.Add(pr1);
		_practitioners.Add(pr2);
		_practitioners.Add(pr3);
	}

	public IEnumerator<Practitioner> GetEnumerator()
	{
		return _practitioners.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private static Practitioner CreatePractitioner1(Organization o11)
	{
		var pr1 = new Practitioner
		{
			Identifier = "4863786296",
			HumanNames =
				new List<HumanName>
				{
					new()
					{
						FamilyName = "Schuler",
						GivenName = "Martin",
						Name = "Martin Schuler",
						Prefix = "Herr Prof. Dr. med.",
						Period = new Period
						{
							Start = DateTime.Parse("01/01/1995", CultureInfo.InvariantCulture)
						}
					}
				},
			Addresses = new List<Address>
			{
				new()
				{
					Text =
						"Universitätsklinikum Essen, Innere Klinik (Tumorforschung), Hufelandstraße 55, 45147 Essen, Deutschland",
					Lines =
						new List<string> { "Universitätsklinikum Essen Innere Klinik (Tumorforschung)" },
					City = "Essen",
					PostalCode = "45147",
					Country = "Deutschland",
					Period = new Period { Start = DateTime.Parse("01/01/1900", CultureInfo.InvariantCulture) }
				}
			},
			Organization = o11
		};
		return pr1;
	}

	private static Practitioner CreatePractitioner2(Organization o2)
	{
		var pr2 = new Practitioner
		{
			Identifier = "6744589222",
			HumanNames = new List<HumanName>
			{
				new()
				{
					FamilyName = "Musterfrau",
					GivenName = "Ulrike",
					Name = "Ulrike Musterfrau",
					Prefix = "Frau Dr.",
					Period = new Period
					{
						Start = DateTime.Parse("01/01/2020", CultureInfo.InvariantCulture)
					}
				}
			},
			Organization = o2
		};
		return pr2;
	}

	private static Practitioner CreatePractitioner3(Organization o3)
	{
		var pr3 = new Practitioner
		{
			Identifier = "2336587441",
			HumanNames = new List<HumanName>
			{
				new()
				{
					FamilyName = "Mustermann",
					GivenName = "Alfred",
					Name = "Alfred Mustermann",
					Prefix = "Herr Dr.",
					Period = new Period
					{
						Start = DateTime.Parse("06/01/2016", CultureInfo.InvariantCulture)
					}
				}
			},
			Organization = o3
		};
		return pr3;
	}
}
