using System.Collections;

namespace SmartHospital.Letters.Fhir.Domain;

public sealed class MockConditionsCollection : IEnumerable<Condition>
{
	private static readonly List<Patient> Patients = new MockPatientsCollection().ToList();
	private static readonly List<Observation> Observations = new MockObservationsCollection().ToList();

	private readonly List<Condition> _conditions = new()
	{
		new Condition
		{
			Identifier = "12345",
			Categories =
				new List<Coding>
				{
					new() { System = "problem-list-item", Code = "123", Display = "Verdacht Hypertension" }
				},
			ClinicalStatus = Enums.ClinicalStatus.Active,
			VerificationStatus = Enums.VerificationStatus.Confirmed,
			Patient = Patients.Single(p => p.Identifier == "1679314678"),
			RecordedDate = new Period { Start = new DateTime(2022, 1, 15) },
			Summary = "Der Patient wurde mit Hypertonie diagnostiziert.",
			Observation = Observations.Single(p => p.Identifier == "678323669"),
			Codes =
				new List<Coding>
				{
					new()
					{
						System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
						Code = "secondary-diagnosis",
						Display = "Arterielle Hypertonie"
					},
					new()
					{
						System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
						Code = "secondary-diagnosis",
						Display = "Hyperurikämie"
					},
					new()
					{
						System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
						Code = "drug-intolerance",
						Display = "Penicillin"
					},
					new()
					{
						System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
						Code = "drug-intolerance",
						Display = "Ibuprofen"
					}
				}
		},
		new Condition
		{
			Identifier = "67890",
			Categories =
				new List<Coding> { new() { System = "problem-list-item", Code = "789", Display = "Verdacht Asthma" } },
			ClinicalStatus = Enums.ClinicalStatus.Active,
			VerificationStatus = Enums.VerificationStatus.Confirmed,
			Patient = Patients.Single(p => p.Identifier == "1679314678"),
			RecordedDate = new Period { Start = new DateTime(2022, 2, 28), End = new DateTime(2022, 4, 14) },
			Summary = "Der Patient wurde mit Asthma diagnostiziert.",
			Observation = Observations.Single(p => p.Identifier == "487164975"),
			Codes =
				new List<Coding>
				{
					new()
					{
						System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
						Code = "secondary-diagnosis",
						Display = "Asthma 2"
					}
				}
		},
		new Condition
		{
			Identifier = "23456",
			Categories =
				new List<Coding>
				{
					new() { System = "problem-list-item", Code = "456", Display = "Verdacht Diabetes" }
				},
			ClinicalStatus = Enums.ClinicalStatus.Active,
			VerificationStatus = Enums.VerificationStatus.Confirmed,
			Patient = Patients.Single(p => p.Identifier == "7656541646"),
			RecordedDate = new Period { Start = new DateTime(2022, 3, 10) },
			Summary = "Der Patient wurde mit Diabetes diagnostiziert.",
			Observation = Observations.Single(p => p.Identifier == "137256485"),
			Codes =
				new List<Coding>
				{
					new()
					{
						System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
						Code = "secondary-diagnosis",
						Display = "Asthma 3"
					}
				}
		},
		new Condition
		{
			Identifier = "78901",
			Categories =
				new List<Coding>
				{
					new() { System = "problem-list-item", Code = "012", Display = "Verdacht Depression" }
				},
			ClinicalStatus = Enums.ClinicalStatus.Active,
			VerificationStatus = Enums.VerificationStatus.Confirmed,
			Patient = Patients.Single(p => p.Identifier == "7656541646"),
			RecordedDate = new Period { Start = new DateTime(2022, 4, 5) },
			Summary = "Der Patient wurde mit Depression diagnostiziert.",
			Observation = Observations.Single(p => p.Identifier == "982365741"),
			Codes =
				new List<Coding>
				{
					new()
					{
						System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
						Code = "secondary-diagnosis",
						Display = "Depression"
					}
				}
		},
		new Condition
		{
			Identifier = "34567",
			Categories =
				new List<Coding>
				{
					new() { System = "problem-list-item", Code = "345", Display = "Verdacht Migraine" }
				},
			ClinicalStatus = Enums.ClinicalStatus.Active,
			VerificationStatus = Enums.VerificationStatus.Confirmed,
			Patient = Patients.Single(p => p.Identifier == "7656541646"),
			RecordedDate = new Period { Start = new DateTime(2022, 5, 20) },
			Summary = "Der Patient wurde mit Migräne diagnostiziert.",
			Observation = Observations.Single(p => p.Identifier == "297245874"),
			Codes = new List<Coding>
			{
				new()
				{
					System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
					Code = "secondary-diagnosis",
					Display = "Migraine"
				}
			}
		}
	};

	public IEnumerator<Condition> GetEnumerator()
	{
		return _conditions.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
