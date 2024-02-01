namespace SmartHospital.Letters.Services.DefaultValues;

internal sealed class SectionNotFoundException : Exception
{
	public SectionNotFoundException(string sectionName)
		: base($"Section {sectionName} not found")
	{
		Data.Add("sectionName", sectionName);
	}
}
