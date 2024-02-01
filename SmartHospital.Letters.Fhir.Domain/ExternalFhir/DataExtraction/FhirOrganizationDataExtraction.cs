using System.Globalization;
using Hl7.Fhir.Rest;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir.DataExtraction;
public class FhirOrganizationDataExtraction
{
	private readonly ExternalFhirClient _externalFhirClient;
	public FhirOrganizationDataExtraction(ExternalFhirClient externalFhirClient)
	{
		_externalFhirClient = externalFhirClient;
	}
	public async Task<List<Organization>> ExtractOrganizationData(SearchParams searchParams)

	{
		List<Hl7.Fhir.Model.Organization> result = await _externalFhirClient.ToListAsync(_externalFhirClient.SearchFhirAsync<Hl7.Fhir.Model.Organization>(searchParams));

		List<Organization> organizations = new List<Organization>();
		foreach (Hl7.Fhir.Model.Organization organization in result)
		{
			Organization org = new Organization();
			if (organization.Type.Count > 0)
			{
				org.Type = new Coding
				{
					System = organization.Type.FirstOrDefault()!.Coding[0].System ?? "",
					Code = organization.Type.FirstOrDefault()!.Coding[0].Code ?? "",
					Display = organization.Type.FirstOrDefault()!.Coding[0].Display ?? ""
				};
			}
			else
			{
				org.Type = new Coding { System = "", Code = "", Display = "" };

			}
			if (organization.Address.Count > 0)
			{

				foreach (Hl7.Fhir.Model.Address address in organization.Address)
				{
					org.Address = new Address
					{

						Period = new Period
						{
							Start = DateTime.Parse(address.Period != null ? address.Period.Start.ToString()! : DateTime.MinValue.ToString(), CultureInfo.InvariantCulture),
							End = DateTime.Parse(address.Period != null ? address.Period.End.ToString()! : DateTime.MinValue.ToString(), CultureInfo.InvariantCulture)
						},
						Text = address.ToString()!,
						Lines = new List<string> { address.Line.First() ?? "" },
						City = address.City != null ? address.City.ToString() : "",
						Country = address.Country != null ? address.ToString()! : "",
						PostalCode = address.PostalCode != null ? address.ToString()! : "",
						State = address.State != null ? address.StateElement.First().Value.ToString()! : ""

					};
				}
			}
			else
			{
				org.Address = new Address();
			}
			org.Name = organization.Name ?? "";
			org.Identifier = organization.Id.ToString() ?? "";
			org.PartOf = new Organization() { Identifier = organization.PartOf != null ? organization.PartOf.Identifier.ToString()! : "" };
			organizations.Add(org);
		}
		return organizations;
	}

}
