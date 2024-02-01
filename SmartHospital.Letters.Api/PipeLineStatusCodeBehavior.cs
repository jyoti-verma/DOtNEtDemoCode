using MediatR;
using SmartHospital.Letters.UseCases;

namespace SmartHospital.Letters.Api;

internal sealed class PipeLineStatusCodeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ILogger<PipeLineStatusCodeBehavior<TRequest, TResponse>> _logger;

	public PipeLineStatusCodeBehavior(ILogger<PipeLineStatusCodeBehavior<TRequest, TResponse>> logger,
		IHttpContextAccessor httpContextAccessor)
	{
		_logger = logger;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		_logger.LogInformation("Handling {Name}", typeof(TRequest).Name);
		TResponse response = await next();

		if (response is not BaseResponse baseResponse)
		{
			return response;
		}

		UpdateResponseStatusCode(baseResponse);
		_logger.LogInformation("Handled {Name}", typeof(TResponse).Name);

		return response;
	}

	private void UpdateResponseStatusCode(BaseResponse baseResponse)
	{
		switch (baseResponse.Code)
		{
			case (int)Codes.NotAuthorized:
			case (int)Codes.UserNotFoundOnLogin:
				_httpContextAccessor.HttpContext!.Response.StatusCode = StatusCodes.Status401Unauthorized;
				break;
			case (int)Codes.LetterDoesNotExists:
			case (int)Codes.SectionDoesNotExists:
				_httpContextAccessor.HttpContext!.Response.StatusCode = StatusCodes.Status404NotFound;
				break;
			case (int)Codes.LetterCreateFailed:
			case (int)Codes.SnippetsCreateFailed:
				_httpContextAccessor.HttpContext!.Response.StatusCode = StatusCodes.Status500InternalServerError;
				break;
			default:
				_httpContextAccessor.HttpContext!.Response.StatusCode = StatusCodes.Status200OK;
				break;
		}
	}
}
