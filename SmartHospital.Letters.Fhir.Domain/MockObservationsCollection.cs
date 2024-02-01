using System.Collections;
using System.Globalization;

namespace SmartHospital.Letters.Fhir.Domain;

public sealed class MockObservationsCollection : IEnumerable<Observation>
{
	private static readonly List<Patient> Patients = new MockPatientsCollection().ToList();
	private static readonly List<Practitioner> Practitioners = new MockPractitionersCollection().ToList();

	private readonly List<Observation> _observations;

	public MockObservationsCollection()
	{
		_observations = new List<Observation>
		{
			new()
			{
				Identifier = "137256485",
				Category = Enums.Categories.Ambulance,
				EffectiveDateTime = DateTime.Parse("08/11/2015", CultureInfo.InvariantCulture),
				Patient = Patients.Single(p => p.Identifier == "1679314678"),
				Performer = Practitioners.Single(p => p.Identifier == "4863786296"),
				Code = new Coding { System = "med", Code = "1245.1", Display = "Erstbegutachtung" },
				Note = "Patientin klagt über Schmerzen beim Einatmen."
			},
			new()
			{
				Identifier = "487164975",
				Category = Enums.Categories.Ambulance,
				EffectiveDateTime = DateTime.Parse("02/06/2016", CultureInfo.InvariantCulture),
				Patient = Patients.Single(p => p.Identifier == "1679314678"),
				Performer = Practitioners.Single(p => p.Identifier == "4863786296"),
				Code = new Coding { System = "med", Code = "1345.1", Display = "Wiedervorstellung" },
				Note = "Patientin klagt wieder über Schmerzen beim Einatmen. Kurzatmigkeit nach Sport."
			},
			new()
			{
				Identifier = "982365741",
				Category = Enums.Categories.Ambulance,
				EffectiveDateTime = DateTime.Parse("06/08/2019", CultureInfo.InvariantCulture),
				Patient = Patients.Single(p => p.Identifier == "7656541646"),
				Performer = Practitioners.Single(p => p.Identifier == "4863786296"),
				Code = new Coding { System = "med", Code = "1345.1", Display = "Notfall" },
				Note = "Patient klagt über Kopfschmerzen"
			},
			new()
			{
				Identifier = "297245874",
				Category = Enums.Categories.Ambulance,
				EffectiveDateTime = DateTime.Parse("06/08/2019", CultureInfo.InvariantCulture),
				Patient = Patients.Single(p => p.Identifier == "1679314678"),
				Performer = Practitioners.Single(p => p.Identifier == "4863786296"),
				Code = new Coding { System = "med", Code = "1345.1", Display = "Notfall" },
				Note = "Patient klagt über Kopfschmerzen"
			},
			new()
			{
				Identifier = "678323669",
				Category = Enums.Categories.Stationary,
				EffectiveDateTime = DateTime.Parse("08/21/2023", CultureInfo.InvariantCulture),
				Patient = Patients.Single(p => p.Identifier == "1679314678"),
				Performer = Practitioners.Single(p => p.Identifier == "6744589222"),
				Code = new Coding
				{
					System = "med", Code = "1345.1", Display = "Aufnahme. Verdacht auf Lungenentzündung"
				},
				Note = "Patient hat blutigen Auswurf beim Husten. Schmerzen im Brustkorb."
			}
		};
	}

	public IEnumerator<Observation> GetEnumerator()
	{
		return _observations.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
