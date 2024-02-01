namespace SmartHospital.Letters.Dtos;

public sealed record SearchResult(string CaseNumber, int AdmissionType, DateTime Date, Patient Patient);
