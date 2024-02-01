using System.Collections.Generic;
using System.Text.Json;
using Fhir.Metrics;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Newtonsoft.Json.Linq;
using SmartHospital.Letters.Fhir.Client;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir;
public class ExternalFhirClient
{
	public string GenerateFhirServerUrl(string? observationIdentifier, string? patientName, Enums.FHIRCallMode fHIRCallMode)
	{
		string baseUrl = FhirConfiguration.FhirExternalURL;
		string queryParams = "_pretty=true";
		if (fHIRCallMode == Enums.FHIRCallMode.Patient)
		{
			baseUrl = baseUrl + "/" + fHIRCallMode.ToString() + "?";
			if (!string.IsNullOrEmpty(patientName))
			{
				queryParams += "&phonetic=" + patientName;
			}
			if (!string.IsNullOrEmpty(observationIdentifier))
			{
				queryParams += "&_id=" + observationIdentifier;
			}
		}
		else if (fHIRCallMode == Enums.FHIRCallMode.Observation)
		{
			baseUrl = baseUrl + "/" + fHIRCallMode.ToString() + "?";
			if (!string.IsNullOrEmpty(observationIdentifier))
			{
				queryParams += "&_id=" + observationIdentifier;
			}
		}
		return baseUrl + queryParams;
	}
	public string GetFhirDataHttpClient(string externalURL)
	{
		try
		{
			var fhirData = System.Threading.Tasks.Task.Run(async () =>
			{
				using (var client = new HttpClient())
				{
					var response = await client.GetAsync(externalURL);

					if (response.IsSuccessStatusCode)
					{
						var content = await response.Content.ReadAsStringAsync();
						return content; // Return the content as a string
					}

					return null; // Handle non-success response
				}
			}).GetAwaiter().GetResult();

			return fhirData;
		}
		catch (Exception ex)
		{
			return "Error";
		}
	}

	public async IAsyncEnumerator<T> SearchFhirAsync<T>(SearchParams searchParams, CancellationToken cancellationToken = default)
	where T : Resource, new()
	{
		ParserSettings parse = new ParserSettings
		{
			TruncateDateTimeToDate = true
		};
		var settings = new FhirClientSettings
		{
			PreferredFormat = ResourceFormat.Json,
			VerifyFhirVersion = true,
			ReturnPreference = ReturnPreference.Minimal
		};
		settings.ParserSettings = parse;
		string FhirServerUrl = FhirConfiguration.FhirExternalURL;
		//string FhirServerUrl = "https://server.fire.ly/";

		BaseFhirClient client = new FhirClient(FhirServerUrl, settings)
			.WithStrictSerializer();

		if (!string.IsNullOrEmpty(searchParams.Query))
		{
			string url= FhirServerUrl + searchParams.Query;	
			yield return await client.ReadAsync<T>(url, ct: cancellationToken)
										 ?? throw new FhirRetrivalException<T>(url);
		}
		else
		{
			Bundle result = client.Search<T>(searchParams);
			foreach (Bundle.EntryComponent? entry in result.Entry)
			{
				if (entry.Resource.TypeName.Contains("OperationOutcome"))
				{
					break;
				}
				yield return await client.ReadAsync<T>(entry.FullUrl, ct: cancellationToken)
							 ?? throw new FhirRetrivalException<T>(entry.FullUrl);
			}
		}
		
	}
	public List<Resource> SearchFhirAsync2(SearchParams searchParams, string ResourceType)
	{
		ParserSettings parse = new ParserSettings
		{
			TruncateDateTimeToDate = true
		};
		var settings = new FhirClientSettings
		{
			PreferredFormat = ResourceFormat.Json,
			VerifyFhirVersion = true,
			ReturnPreference = ReturnPreference.Minimal
		};
		settings.ParserSettings = parse;

		var pageSize = 100;
		var offset = 0;
		string FhirServerUrl = FhirConfiguration.FhirExternalURL;

		BaseFhirClient client = new FhirClient(FhirServerUrl, settings)
			.WithStrictSerializer();

		var allRecords = new List<Resource>();


		while (true)
		{
			searchParams.Where("_count=" + pageSize).Where("_offset=" + offset);
			Bundle bundle = client.Search(searchParams, ResourceType);

			// Add the fetched records to the list
			allRecords.AddRange(bundle.Entry.Select(entry => entry.Resource));

			// Check if there are more pages to fetch
			if (bundle.NextLink == null || bundle.Entry.Count< offset)
			{
				break;
			}

			// Update the offset for the next page
			offset += pageSize;

		}
		return allRecords;
	}
	public string ReadDataFromJson()
	{
		string firstFile = string.Empty;
		// Specify the path to your JSON file
		string folderPath = "C:\\Users\\jverma28\\Downloads\\RD";
		// Check if the folder exists
		if (Directory.Exists(folderPath))
		{
			// Get all files in the folder
			string[] files = Directory.GetFiles(folderPath);

			// Check if there are any files in the folder
			if (files.Length > 0)
			{
				// Pick the first file
				 firstFile = files.First();

				// Now you can work with the path of the first file
				Console.WriteLine($"First File: {firstFile}");
			}
			else
			{
				Console.WriteLine("No files found in the folder.");
			}
		}
		else
		{
			Console.WriteLine("Folder not found.");
		}
		// Read the entire file into a string
		string jsonString = File.ReadAllText(firstFile);

		// Deserialize the JSON string into a C# object
		//JObject myObject = JsonSerializer.Deserialize<JObject>(jsonString);
		return jsonString;
		
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
