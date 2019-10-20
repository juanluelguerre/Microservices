using ElGuerre.Microservices.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace ElGuerre.Microservices.Shared.Behaviors
{
	public class ValidatorPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	{
		private readonly ILogger<ValidatorPipelineBehavior<TRequest, TResponse>> _logger;
		private readonly IValidator<TRequest>[] _validators;

		public ValidatorPipelineBehavior(ILogger<ValidatorPipelineBehavior<TRequest, TResponse>> logger, IValidator<TRequest>[] validators)
		{
			_validators = validators;
			_logger = logger;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			var typeName = request.GetGenericTypeName();

			_logger.LogInformation("----- Validating command {CommandType}", typeName);

			var failures = _validators
				.Select(v => v.Validate(request))
				.SelectMany(result => result.Errors)
				.Where(error => error != null)
				.ToList();

			if (failures.Any())
			{
				_logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

				// TODO: Review
				//throw new OrderingException($"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));
				throw new ValidationException($"Command Validation Errors for type { typeof(TRequest).Name}", failures);
			}

			return await next();
		}
	}
}
