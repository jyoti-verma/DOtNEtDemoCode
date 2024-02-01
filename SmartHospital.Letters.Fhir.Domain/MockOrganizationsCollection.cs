using System.Collections;
using System.Globalization;

namespace SmartHospital.Letters.Fhir.Domain;

public sealed class MockOrganizationsCollection : IEnumerable<Organization>
{
	private readonly List<Organization> _organizations;

	public MockOrganizationsCollection()
	{
		_organizations = new List<Organization>();
		CreateOrganizations();
	}

	public IEnumerator<Organization> GetEnumerator()
	{
		return _organizations.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void CreateOrganizations()
	{
		Organization o1A = CreateOrganization1A();
		Organization o1B = CreateOrganization1B(o1A);
		Organization o2 = CreateOrganization2();
		Organization o3 = CreateOrganization3();
		Organization o4 = CreateOrganization4();

		_organizations.Add(o1A);
		_organizations.Add(o1B);
		_organizations.Add(o2);
		_organizations.Add(o3);
		_organizations.Add(o4);
	}

	private static Organization CreateOrganization1A()
	{
		var o1A = new Organization
		{
			Identifier = "1487457485",
			Type = new Coding { System = "xyz", Code = "cli", Display = "Klinik" },
			Address = new Address
			{
				Text = "Universitätsklinikum Essen (AöR) Hufelandstraße 55 45147 Essen, Deutschland",
				Lines = new List<string> { "Hufelandstraße 55" },
				City = "Essen",
				PostalCode = "45147",
				Country = "Deutschland",
				Period = new Period { Start = DateTime.Parse("01/01/1900", CultureInfo.InvariantCulture) }
			},
			Name = "Universitätsklinikum Essen"
		};
		return o1A;
	}

	private static Organization CreateOrganization1B(Organization o1A)
	{
		var o1B = new Organization
		{
			Identifier = "1379584647",
			Type = new Coding { System = "xyz", Code = "dep", Display = "Abteilung" },
			Address = new Address
			{
				Text =
					"Universitätsklinikum Essen, Innere Klinik (Tumorforschung), Hufelandstraße 55, 45147 Essen, Deutschland",
				Lines = new List<string> { "Hufelandstraße 55" },
				City = "Essen",
				PostalCode = "45147",
				Country = "Deutschland",
				Period = new Period { Start = DateTime.Parse("01/01/1900", CultureInfo.InvariantCulture) }
			},
			Name = "Universitätsklinikum Essen Innere Klinik",
			PartOf = o1A
		};
		return o1B;
	}

	private static Organization CreateOrganization2()
	{
		Organization o2;
		o2 = new Organization
		{
			Identifier = "4785693214",
			Type = new Coding { System = "xyz", Code = "prx", Display = "Radiologie" },
			Address = new Address
			{
				Text = "Theodor-Heuss-Str. 3290, 45127 Essen, Deutschland",
				Lines = new List<string> { "Theodor-Heuss-Str. 3290" },
				City = "Essen",
				PostalCode = "45127",
				Country = "Deutschland",
				Period = new Period { Start = DateTime.Parse("01/01/1995", CultureInfo.InvariantCulture) }
			},
			Name = "Radiologie Dr. Ulrike Musterfrau"
		};
		return o2;
	}

	private static Organization CreateOrganization3()
	{
		var o3 = new Organization
		{
			Identifier = "6745893121",
			Type = new Coding { System = "xyz", Code = "prx", Display = "Hausarzt" },
			Address = new Address
			{
				Text = "Hauptstraße 32, 45127 Essen, Deutschland",
				Lines = new List<string> { "Hauptstraße 32" },
				City = "Essen",
				PostalCode = "45127",
				Country = "Deutschland",
				Period = new Period { Start = DateTime.Parse("12/01/1999", CultureInfo.InvariantCulture) }
			},
			Name = "Musterpraxis Dr. Alfred Mustermann"
		};
		return o3;
	}

	private static Organization CreateOrganization4()
	{
		var o4 = new Organization
		{
			Identifier = "573684178",
			Type = new Coding { System = "xyz", Code = "llb", Display = "Labor" },
			Address = new Address
			{
				Text = "Huttropstraße 58, 45138 Essen, Deutschland",
				Lines = new List<string> { "Huttropstraße 58" },
				City = "Essen",
				PostalCode = "45138",
				Country = "Deutschland",
				Period = new Period { Start = DateTime.Parse("01/01/2000", CultureInfo.InvariantCulture) }
			},
			Name = "mvzlm Ruhr"
		};
		return o4;
	}
}
