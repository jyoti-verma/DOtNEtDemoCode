namespace SmartHospital.Letters.UseCases;

public record BaseResponse(
	string? Message = "Success",
	int Code = 0
);

public record BaseResponse<T>(
	T? Value,
	string Message = "Success",
	int Code = 0
) : BaseResponse(Message, Code);
