namespace SmartHospital.Letters.Entities;

public interface IBaseClass : IIdentifier<Guid>
{
	DateTime Created { get; set; }
	string CreatedBy { get; set; }
	DateTime Modified { get; set; }
	string ModifiedBy { get; set; }
}
