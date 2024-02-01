using System.Net;
using MediatR;
using Refit;
using SmartHospital.Letters.Core.Extensions;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.UseCases.Search;

public sealed class SearchHandler : IRequestHandler<SearchRequest, SearchResponse>
{
	private readonly IFhirApiClient _fhirMockApiClient;

	public SearchHandler(IFhirApiClient fhirMockApiClient)
	{
		_fhirMockApiClient = fhirMockApiClient;
	}

	public async Task<SearchResponse> Handle(SearchRequest request, CancellationToken cancellationToken)
	{
		IEnumerable<ObservationDto> observations;

		try
		{
			if (int.TryParse(request.Text.RemoveWhitespace(), out _))
			{
				// Search for CaseNr
				observations = new List<ObservationDto>
				{
					await _fhirMockApiClient.GetObservation(
						request.Text.RemoveWhitespace(),
						cancellationToken
					)
				};
			}
			else
			{
				// Search for Names
				IEnumerable<PatientDto> patients =
					await _fhirMockApiClient.GetPatients(request.Text, cancellationToken: cancellationToken);

				var tasks = patients
					.Select(patient => _fhirMockApiClient.GetObservations(patient.Identifier, cancellationToken))
					.ToList();

				await Task.WhenAll(tasks);

				observations = tasks
					.Select(task => task.Result)
					.SelectMany(observation => observation)
					.ToList();
			}
		}
		catch (ValidationApiException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
		{
			observations = Enumerable.Empty<ObservationDto>();
		}

		IEnumerable<Task<SearchResult>> searchTasks = observations.Select(async observation =>
		{
			PatientDto patientDto = await _fhirMockApiClient.GetPatient(
				observation.PatientIdentifier,
				cancellationToken
			);

			return new SearchResult(
				observation.Identifier,
				observation.Category,
				observation.EffectiveDateTime,
				new Patient(patientDto)
			);
		});

		SearchResult[] searchResults = await Task.WhenAll(searchTasks);

		return searchResults.Any()
			? new SearchResponse(searchResults)
			: new SearchResponse(
				(int)Codes.PatientCasesNotFound,
				"Nothing found"
			);
	}
}
