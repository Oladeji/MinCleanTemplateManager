
using LanguageExt;
using CQRSHelper;

using Microsoft.Extensions.Logging;
using DomainErrors;

namespace MinCleanTemplateManager.Application.Behaviours
{
    public class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
           where TResponse : IEither // Specify the type of Result 
    {
        private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;

        public LoggingPipelineBehaviour(ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // This can be used to log request and responses through the application

            _logger.LogInformation("Starting request {@RequestName}, {@DateTimeUtc} ", typeof(TRequest).Name, DateTime.UtcNow);

            var result = await next();
            if (result.IsLeft)
            {
                result.MatchUntyped(_ => CreateSuccessLog(result), error => CreateErrorLog(error));
                _logger.LogError("Error occured in request {@RequestName}, {@DateTimeUtc} ", typeof(TRequest).Name, DateTime.UtcNow);


            }

            _logger.LogInformation("Completed request {@RequestName}, {@DateTimeUtc} ", typeof(TRequest).Name, DateTime.UtcNow);


            return result;
        }

        private object CreateErrorLog(object theerror)
        {
            try
            {
                var error = (GeneralFailure)theerror;

         
                _logger.LogError("Request failure  {@RequestName} {@DateTimeUtc}  :, {@ErrorCode},{@ErrorType},{@ErrorDescription}", typeof(TRequest).Name, DateTime.UtcNow, error.Code, error.OriginalError, error.ErrorDescription);

            }
            catch (Exception ex)
            {

                throw;
            }
            return theerror;
        }

        private object CreateSuccessLog(object result)
        {
            try
            {

                _logger.LogInformation("{@DateTimeUtc} Completed request {@RequestName}  was successful ", typeof(TRequest).Name, typeof(TRequest).Name);

            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }
    }
}
