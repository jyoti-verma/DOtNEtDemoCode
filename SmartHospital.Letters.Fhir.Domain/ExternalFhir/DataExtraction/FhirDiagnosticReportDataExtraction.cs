using System.Globalization;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using static SmartHospital.Letters.Fhir.Domain.Enums;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir.DataExtraction;
public class FhirDiagnosticReportDataExtraction
{
	private readonly ExternalFhirClient _externalFhirClient;
	private readonly FhirObservationDataExtraction _fhirObservationDataExtraction;
	public FhirDiagnosticReportDataExtraction(ExternalFhirClient externalFhirClient, FhirObservationDataExtraction fhirObservationDataExtraction)
	{
		_externalFhirClient = externalFhirClient;
		_fhirObservationDataExtraction = fhirObservationDataExtraction;
	}
	public async Task<List<DiagnosticReport>> ExtractDiagnosticReportData(SearchParams searchParams)
	{


		List<Hl7.Fhir.Model.DiagnosticReport> result = await _externalFhirClient.ToListAsync(_externalFhirClient.SearchFhirAsync<Hl7.Fhir.Model.DiagnosticReport>(searchParams));

		List<DiagnosticReport> diagnosticReports = new List<DiagnosticReport>();
		foreach (Hl7.Fhir.Model.DiagnosticReport diagnosticreport in result)
		{
			var t = diagnosticreport.ResourceIdentity().UserInfo.ToString();
			DiagnosticReport report = new DiagnosticReport();

			report.Identifier = diagnosticreport.Id;
			report.CollectedDateTime = new DateTime(2023, 8, 22);
			if (diagnosticreport.Effective != null)
			{
				if (diagnosticreport.Effective.Count() > 1)
				{
					report.EffectiveDateTime = DateTime.Parse(DateTime.MinValue.ToString(), CultureInfo.InvariantCulture);
				}
				else
				{
					report.EffectiveDateTime = DateTime.Parse(diagnosticreport.Effective != null ?
					diagnosticreport.Effective.FirstOrDefault().Value.ToString()! :
					DateTime.MinValue.ToString(), CultureInfo.InvariantCulture);
				}

			}
			else
			{
				report.EffectiveDateTime = DateTime.Parse(DateTime.MinValue.ToString(), CultureInfo.InvariantCulture);

			}

			report.HeaderDiagnosis = "Nicht-kleinzelliges Lungenka…";
			report.EcogPerformanceStatus = 1;
			//report.SpecimenMethod = diagnosticreport.Specimen != null ? (SpecimenMethod)diagnosticreport.Specimen:SpecimenMethod.Unknown;

			report.SpecimenMethod = SpecimenMethod.Unknown;
			report.TumorEntity = new Coding { System = "", Code = "", Display = "" };
			//if (diagnosticreport.TumorEntity != null)
			//{
			//	report.TumorEntity = new Coding
			//	{
			//		System = diagnosticreport.TumorEntity.System ?? "",
			//		Code = diagnosticreport.TumorEntity.Code ?? "",
			//		Display = diagnosticreport.TumorEntity.Display ?? ""
			//	};
			//}
			//if (diagnosticreport.TumorMorphology != null)
			//{
			//	report.TumorMorphology = new Coding
			//	{
			//		System = diagnosticreport.TumorMorphology.System ?? "",
			//		Code = diagnosticreport.TumorMorphology.Code ?? "",
			//		Display = diagnosticreport.TumorMorphology.Display ?? ""
			//	};
			//}
			report.TumorMorphology = new Coding { System = "", Code = "", Display = "" };

			//if (diagnosticreport.TumorHistology != null)
			//{
			//	report.TumorHistology = new Coding
			//	{
			//		System = diagnosticreport.TumorHistology.System ?? "",
			//		Code = diagnosticreport.TumorHistology.Code ?? "",
			//		Display = diagnosticreport.TumorHistology.Display ?? ""
			//	};
			//}
			report.TumorHistology = new Coding { System = "", Code = "", Display = "" };

			//if (diagnosticreport.MolecularPathologyFindings != null)
			//{
			//	foreach (var molecule in report.MolecularPathologyFindings)
			//	{
			//		report.MolecularPathologyFindings = new List<Coding>
			//		{
			//			new()
			//			{
			//				System = molecule.System ?? "",
			//				Code = molecule.Code ?? "",
			//				Display = molecule.Display ?? "",
			//			}
			//		};

			//	}
			//}
			report.MolecularPathologyFindings = new List<Coding>
				{
					new()
						{
							System =  "",
							Code =  "",
							Display = "",
						}
				};
			//if (diagnosticreport.TumorStadium != null)
			//{
			//	foreach (var tumor in report.TumorStadium)
			//	{
			//		report.MolecularPathologyFindings = new List<Coding>
			//		{
			//			new()
			//			{
			//				System = tumor.System ?? "",
			//				Code = tumor.Code ?? "",
			//				Display = tumor.Display ?? "",
			//			}
			//		};

			//	}
			//}
			report.TumorStadium = new List<Coding>
				{
					new()
						{
							System =  "",
							Code =  "",
							Display = "",
						}
				};
			var obs = string.Empty;
			if (diagnosticreport.Result.Count > 0&&diagnosticreport.Result.FirstOrDefault().Url.OriginalString.Contains("_history"))
			{
				 obs = diagnosticreport.Result.Count > 0 ? diagnosticreport.Result.FirstOrDefault().Url.OriginalString.Split("/_history")[0]:"";
				report.Observation = new Observation { Identifier = diagnosticreport.Result.Count > 0 ? obs.Split("Observation/")[1] : "" };

			}
			else
			{
				report.Observation = new Observation { Identifier = diagnosticreport.Result.Count > 0 ? diagnosticreport.Result.FirstOrDefault().Url.OriginalString.Split("Observation/")[1] : "" };
			}
			report.Patient = new Patient() { Identifier = diagnosticreport.Subject != null ? diagnosticreport.Subject.Url.OriginalString.Split("Patient/")[1] : "" };
			report.Performer = new Organization() { Identifier = diagnosticreport.Performer.Count > 0 ? diagnosticreport.Performer.FirstOrDefault().Url.OriginalString.Split("Practitioner/")[1] : "" };

			diagnosticReports.Add(report);
		}
		return diagnosticReports;
	}
}
