using MediatR;
using Microsoft.Extensions.Logging;

namespace SmartHospital.Letters.UseCases;

public class PipeLineLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
{
	private readonly ILogger<PipeLineLoggingBehavior<TRequest, TResponse>> _logger;

	public PipeLineLoggingBehavior(ILogger<PipeLineLoggingBehavior<TRequest, TResponse>> logger)
	{
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		_logger.LogInformation($"Handling {typeof(TRequest).Name}");
		TResponse response = await next();
		_logger.LogInformation($"Handled {typeof(TResponse).Name}");

		return response;
	}
}
