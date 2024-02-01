using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Repositories;
using SmartHospital.Letters.Services.Extensions;

namespace SmartHospital.Letters.Services.DefaultValues;

internal sealed class DefaultLetterTypes : IDefaultValues
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILetterTypeRepository _letterTypeRepository;
	private readonly SystemUser _user;

	public DefaultLetterTypes(ILetterTypeRepository letterTypeRepository, IDateTimeProvider dateTimeProvider)
	{
		_letterTypeRepository = letterTypeRepository;
		_dateTimeProvider = dateTimeProvider;
		_user = new SystemUser();
	}

	public async Task CreateAsync(CancellationToken cancellationToken = default)
	{
		ICollection<LetterType> letterTypes = new List<LetterType>
		{
			new(LetterTypes.Therapy, _dateTimeProvider.Now, _user.UserName!,
				new Guid("b160605f-bf4e-40ec-ae87-bd2d5d206fbc")),
			new(LetterTypes.ImagingFollowUpControl, _dateTimeProvider.Now, _user.UserName!,
				new Guid("9a19efa0-6db5-4fa0-bc97-7433c2db8463")),
			new(LetterTypes.Aftercare, _dateTimeProvider.Now, _user.UserName!,
				new Guid("2e82b2fb-4343-4c92-8a43-0d576bffcb8e")),
			new(LetterTypes.TerminationOfTumorRelatedTherapy, _dateTimeProvider.Now, _user.UserName!,
				new Guid("3b288a49-6539-4a89-9749-a88777886979"))
		};

		foreach (LetterType letterType in letterTypes)
		{
			await _letterTypeRepository.TryAddAsync(letterType, cancellationToken);
		}
	}
}
