using AutoMapper;
using SmartHospital.Letters.Fhir.Domain;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Fhir.Mock.Api;

internal sealed class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<FhirResource, FhirResourceDto>();

		CreateMap<Coding, CodingDto>();

		CreateMap<Period, PeriodDto>();

		CreateMap<HumanName, HumanNameDto>();

		CreateMap<Address, AddressDto>();

		CreateMap<Person, PersonDto>();

		CreateMap<Patient, PatientDto>();

		CreateMap<Organization, OrganizationDto>()
			.ForPath(d => d.PartOfOrganizationIdentifier, opt =>
				opt.MapFrom(s => s.PartOf != null ? s.PartOf.Identifier : ""));

		CreateMap<Observation, ObservationDto>()
			.ForPath(d => d.PatientIdentifier, opt =>
				opt.MapFrom(s => s.Patient.Identifier))
			.ForPath(d => d.PerformerIdentifier, opt =>
				opt.MapFrom(s => s.Performer != null ? s.Performer.Identifier : ""));

		CreateMap<Practitioner, PractitionerDto>()
			.ForPath(d => d.OrganizationIdentifier, opt =>
				opt.MapFrom(s => s.Organization.Identifier));

		CreateMap<Condition, ConditionDto>()
			.ForPath(d => d.PatientIdentifier, opt =>
				opt.MapFrom(s => s.Patient.Identifier))
			.ForPath(d => d.ObservationIdentifier, opt =>
				opt.MapFrom(s => s.Observation.Identifier));

		CreateMap<DiagnosticReport, DiagnosticReportDto>()
			.ForPath(d => d.PatientIdentifier, opt =>
				opt.MapFrom(s => s.Patient.Identifier))
			.ForPath(d => d.ObservationIdentifier, opt =>
				opt.MapFrom(s => s.Observation.Identifier))
			.ForPath(d => d.PerformerIdentifier, opt =>
				opt.MapFrom(s => s.Performer.Identifier));
	}
}
