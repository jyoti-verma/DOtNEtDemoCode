using System.Collections;
using System.Globalization;

namespace SmartHospital.Letters.Fhir.Domain;

public sealed class MockPatientsCollection : IEnumerable<Patient>
{
	private readonly List<Patient> _patients = new()
	{
		CreatePatient1(), CreatePatient2(), CreatePatient3(), CreatePatient4()
	};

	public IEnumerator<Patient> GetEnumerator()
	{
		return _patients.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private static Patient CreatePatient1()
	{
		var hn1 = new HumanName
		{
			GivenName = "Susanne",
			FamilyName = "Petermann",
			Period = new Period { Start = DateTime.Parse("01/01/2000", CultureInfo.InvariantCulture) },
			Name = "Susanne Petermann",
			Prefix = ""
		};

		var a1 = new Address
		{
			Period = new Period { Start = DateTime.Parse("01/01/2000", CultureInfo.InvariantCulture) },
			Text = "Pasteurstr. 65, 45458 Dortmund, Deutschland",
			Lines = new List<string> { "Pasteurstr. 65" },
			City = "Dortmund",
			Country = "Deutschland",
			PostalCode = "45458",
			State = "NRW"
		};
		var p1 = new Patient
		{
			Identifier = "7656541646",
			HumanNames = new List<HumanName> { hn1 },
			CityOfBirth = "Essen",
			DateOfBirth = DateTime.Parse("08/16/1970", CultureInfo.InvariantCulture),
			Gender = Enums.Genders.Female,
			Deceased = false,
			Addresses = new List<Address> { a1 }
		};
		return p1;
	}

	private static Patient CreatePatient2()
	{
		var hn2 = new HumanName
		{
			GivenName = "Michaela",
			FamilyName = "Schneider",
			Period = new Period { Start = DateTime.Parse("01/01/2000", CultureInfo.InvariantCulture) },
			Name = "Michaela Schneider",
			Prefix = ""
		};

		var l2 = new List<string> { "Musterstr. 12" };
		var a2 = new Address
		{
			Period = new Period { Start = DateTime.Parse("01/01/2000", CultureInfo.InvariantCulture) },
			Text = "Musterstr. 12, 45127 Essen, Deutschland",
			Lines = l2,
			City = "Essen",
			Country = "Deutschland",
			PostalCode = "45127",
			State = "NRW"
		};

		var p2 = new Patient
		{
			Identifier = "1679314678",
			HumanNames = new List<HumanName> { hn2 },
			CityOfBirth = "Essen",
			DateOfBirth = DateTime.Parse("09/13/1982", CultureInfo.InvariantCulture),
			Gender = Enums.Genders.Female,
			Deceased = false,
			Addresses = new List<Address> { a2 }
		};
		return p2;
	}

	private static Patient CreatePatient3()
	{
		var hn3 = new HumanName
		{
			GivenName = "Gustav",
			FamilyName = "Grün",
			Period = new Period { Start = DateTime.Parse("01/01/2000", CultureInfo.InvariantCulture) },
			Name = "Gustav Grün",
			Prefix = "Dr."
		};

		var l3 = new List<string> { "Hauptstr. 167" };
		var a3 = new Address
		{
			Period = new Period { Start = DateTime.Parse("01/01/2000", CultureInfo.InvariantCulture) },
			Text = "Hauptstr. 167, 45127 Essen, Deutschland",
			Lines = l3,
			City = "Essen",
			Country = "Deutschland",
			PostalCode = "45127",
			State = "NRW"
		};

		var p3 = new Patient
		{
			Identifier = "5296463187",
			HumanNames = new List<HumanName> { hn3 },
			CityOfBirth = "Leverkusen",
			DateOfBirth = DateTime.Parse("03/19/1958", CultureInfo.InvariantCulture),
			Gender = Enums.Genders.Male,
			Deceased = false,
			Addresses = new List<Address> { a3 }
		};
		return p3;
	}

	private static Patient CreatePatient4()
	{
		var hn4 = new HumanName
		{
			GivenName = "Köhler",
			FamilyName = "Klaus",
			Period = new Period { Start = DateTime.Parse("01/01/1999", CultureInfo.InvariantCulture) },
			Name = "Klaus Köhler",
			Prefix = ""
		};

		var l4 = new List<string> { "Aachener Straße 34" };
		var a4 = new Address
		{
			Period = new Period { Start = DateTime.Parse("09/01/2019", CultureInfo.InvariantCulture) },
			Text = "Aachener Straße 34, 56487 Bonn, Deutschland",
			Lines = l4,
			City = "Bonn",
			Country = "Deutschland",
			PostalCode = "56487",
			State = "NRW"
		};

		var p4 = new Patient
		{
			Identifier = "5783146978",
			HumanNames = new List<HumanName> { hn4 },
			CityOfBirth = "Bonn",
			DateOfBirth = DateTime.Parse("02/28/1991", CultureInfo.InvariantCulture),
			Gender = Enums.Genders.Male,
			Deceased = false,
			Addresses = new List<Address> { a4 }
		};
		return p4;
	}
}
