namespace SmartHospital.Letters.UseCases;

public enum Codes
{
	NotAuthorized = 4711,
	UserNotFoundOnLogin = 4040,
	PatientNotFound = 4041,
	PatientCasesNotFound = 4042,
	LetterTypeNotFound = 4043,
	NoLettersFound = 4050,
	LetterDoesNotExists = 4051,
	SectionDoesNotExists = 4052,
	LetterCreateFailed = 4060,
	SnippetsCreateFailed = 4070,
	Success = 0
}
