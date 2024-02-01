using System.Globalization;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using static SmartHospital.Letters.Fhir.Domain.Enums;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir.DataExtraction;
public class FhirConditionDataExtraction
{
	private readonly ExternalFhirClient _externalFhirClient;

	public FhirConditionDataExtraction(ExternalFhirClient externalFhirClient)
	{
		_externalFhirClient = externalFhirClient;
	}
	public async Task<List<Condition>> ExtractConditionData(SearchParams searchParams)
	{
		List<Condition> conditions = new List<Condition>();
		List<Hl7.Fhir.Model.Condition> result = await _externalFhirClient.ToListAsync(_externalFhirClient.SearchFhirAsync<Hl7.Fhir.Model.Condition>(searchParams));

		
			foreach (Hl7.Fhir.Model.Condition condition in result)
			{
				var isExistcategory = condition.GetType().GetProperty("Category");
				var isExistClinicalStatus = condition.GetType().GetProperty("ClinicalStatus");
				var isExistVerificationStatus = condition.GetType().GetProperty("VerificationStatus");
				var isExistRecordedDate = condition.GetType().GetProperty("RecordedDate");
				Condition con = new Condition();
				con.Identifier = condition.GetType().GetProperty("Id") != null ? condition.Id : "";
				if (isExistcategory != null && condition.Category != null)
				{
					var Coding = condition.Category.GetType().GetProperty("Coding");

					if (Coding == null)
					{
						con.Categories = new List<Coding> { new() { System = "", Code = "", Display = "" } };
					}
					else
					{
						foreach (var code in condition.Category.FirstOrDefault().Coding)
						{
							Coding co = new Coding
							{
								System = code.System ?? "",
								Code = code.Code ?? "",
								Display = code.Display ?? ""
							};
							con.Categories.Add(co);
						}

					}
				}
				else
				{
					con.Categories = new List<Coding> { new() { System = "", Code = "", Display = "" } };

				}
				if (isExistClinicalStatus != null && condition.ClinicalStatus != null)
				{
					var Coding = condition.ClinicalStatus.GetType().GetProperty("Coding");
					var Code = condition.ClinicalStatus.Coding.GetType().GetProperty("Code");

					con.ClinicalStatus = Enum.TryParse(condition.ClinicalStatus.Coding.First().Code, true, out ClinicalStatus clinicalStatus) ?
						clinicalStatus : ClinicalStatus.Unknown;
				}
				if (isExistVerificationStatus != null && condition.VerificationStatus != null)
				{
					con.VerificationStatus = Enum.TryParse(condition.VerificationStatus.Coding.First().Code, true, out VerificationStatus verificationStatus) ?
						verificationStatus : VerificationStatus.Unknown;
				}
				if (isExistRecordedDate != null && condition.RecordedDate != null)
				{
					con.RecordedDate = new Period
					{
						Start = System.DateTime.Parse(condition.RecordedDate.ToString(), CultureInfo.InvariantCulture),
					};
				}
				else
				{
					con.RecordedDate = new Period
					{
						Start = System.DateTime.Parse(System.DateTime.MinValue.ToString(), CultureInfo.InvariantCulture),
					};
				}
				con.Summary = condition.Note.Count > 0 ? condition.Note.First().Text.ToString() : "";
				if (condition.Code.Coding.Count > 0)
				{
					foreach (var code in condition.Code.Coding)
					{
						Coding co = new Coding
						{
							System = code.System ?? "",
							Code = code.Code ?? "",
							Display = code.Display ?? ""
						};
						con.Codes.Add(co);
					}
				}
				else
				{
					con.Categories = new List<Coding> { new() { System = "", Code = "", Display = "" } };
				}
				con.Patient = new Patient() { Identifier = condition.Subject != null ? condition.Subject.Url.OriginalString.Split("Patient/")[1] : "" };
				if (condition.Evidence.Count > 0)
				{
					if (condition.Evidence.First().Detail.Count > 0 && condition.Evidence.First().Detail.First().Url != null)
					{
						con.Observation = new Observation() { Identifier = condition.Evidence.First().Detail.First().Url.OriginalString.Split("Observation/")[1] };
					}
					else
					{
						con.Observation = new Observation() { Identifier = "" };
					}
				}
				else
				{
					con.Observation = new Observation() { Identifier = "" };
				}
				conditions.Add(con);
			}
		

		return conditions;
	}
}
