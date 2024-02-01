using System.Collections;

namespace SmartHospital.Letters.Fhir.Domain;

public sealed class MockDiagnosticReportCollection : IEnumerable<DiagnosticReport>
{
	private static readonly List<Patient> Patients = new MockPatientsCollection().ToList();
	private static readonly List<Observation> Observations = new MockObservationsCollection().ToList();
	private static readonly List<Organization> Organizations = new MockOrganizationsCollection().ToList();

	private readonly List<DiagnosticReport> _diagnosticReports = new()
	{
		CreateDiagnosticReport1(), CreateDiagnosticReport2()
	};

	public IEnumerator<DiagnosticReport> GetEnumerator()
	{
		return _diagnosticReports.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private static DiagnosticReport CreateDiagnosticReport1()
	{
		var report1 = new DiagnosticReport
		{
			Identifier = "753426819",
			Patient = Patients.Single(p => p.Identifier == "1679314678"), // Michaela Schneider
			Observation = Observations.Single(p => p.Identifier == "678323669"), // Lungenentzündung?
			Performer = Organizations.Single(p => p.Identifier == "573684178"), // mvzlm Ruhr
			CollectedDateTime = new DateTime(2023, 8, 22),
			EffectiveDateTime = new DateTime(2023, 8, 30),
			HeaderDiagnosis = "Nicht-kleinzelliges Lungenka…",
			SpecimenMethod = Enums.SpecimenMethod.Biopsy,
			TumorEntity =
				new Coding
				{
					System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
					Code = "C34.3",
					Display = "Bösartige Neubildung: Unterlappen (-Bronchus)"
				},
			TumorMorphology =
				new Coding { System = "http://terminology.hl7.org/CodeSystem/icd-o-3", Code = "-", Display = "-" },
			TumorHistology =
				new Coding { System = "xyr", Code = "ttt", Display = "Pulmonales Adenokarzinom (Prof. Schmid)" },
			MolecularPathologyFindings =
				new List<Coding>
				{
					new() { System = "aaa", Code = "p.L597Q", Display = "BRAF-Mutation p.L597Q" },
					new()
					{
						System = "aaa",
						Code = "bbb",
						Display = "Mutationnen in TP53, SKT11, KEAP1 (Prof. Schmid)"
					}
				},
			TumorStadium = new List<Coding>
			{
				new() { System = "ccc", Code = "ddd", Display = "TNM (initial): cT3 CN3, cM1c (OSS, PLE, OTH)" },
				new() { System = "ccc", Code = "eee", Display = "Stadium (initial): St. IVB nach UICC 2016" }
			},
			EcogPerformanceStatus = 1
		};

		return report1;
	}

	private static DiagnosticReport CreateDiagnosticReport2()
	{
		var report2 = new DiagnosticReport
		{
			Identifier = "447931024",
			Patient = Patients.Single(p => p.Identifier == "1679314678"), // Michaela Schneider
			Observation = Observations.Single(p => p.Identifier == "678323669"), // Lungenentzündung?
			Performer = Organizations.Single(p => p.Identifier == "573684178"), // mvzlm Ruhr
			CollectedDateTime = new DateTime(2023, 8, 22),
			EffectiveDateTime = new DateTime(2023, 8, 30),
			HeaderDiagnosis = "Weiteres Karzinom",
			SpecimenMethod = Enums.SpecimenMethod.Biopsy,
			TumorEntity =
				new Coding
				{
					System = "http://fhir.de/CodeSystem/dimdi/icd-10-gm",
					Code = "C34.8",
					Display = "Bösartige Neubildung: Bronchus und Lunge, mehrere Teilbereiche überlappend"
				},
			TumorMorphology =
				new Coding { System = "http://terminology.hl7.org/CodeSystem/icd-o-3", Code = "-", Display = "-" },
			TumorHistology =
				new Coding { System = "xyr", Code = "ttt", Display = "Pulmonales Adenokarzinom (Prof. Schmid)" },
			MolecularPathologyFindings =
				new List<Coding>
				{
					new() { System = "aaa", Code = "p.L597Q", Display = "BRAF-Mutation p.L597Q" },
					new()
					{
						System = "aaa",
						Code = "bbb",
						Display = "Mutationnen in TP53, SKT11, KEAP1 (Prof. Schmid)"
					}
				},
			TumorStadium = new List<Coding>
			{
				new() { System = "ccc", Code = "ddd", Display = "TNM (initial): cT3 CN3, cM1c (OSS, PLE, OTH)" },
				new() { System = "ccc", Code = "eee", Display = "Stadium (initial): St. IVB nach UICC 2016" }
			},
			EcogPerformanceStatus = 2
		};

		return report2;
	}
}
