namespace SmartHospital.Letters.Dtos;

public record CaseSearchResponse(
	string CaseNumber,
	string AdmissionType,
	DateTime Date,
	Patient Patient);
