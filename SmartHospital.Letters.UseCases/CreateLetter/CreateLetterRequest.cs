using MediatR;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.UseCases.CreateLetter;

public sealed record CreateLetterRequest(
	string LetterTypeName,
	AdmissionTypes AdmissionType,
	string ExternalPatientId,
	string ExternalCaseNumber,
	LetterUser User
) : IRequest<CreateLetterResponse>;
