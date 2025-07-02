using LanguageExt;

namespace CQRSHelper
{
    /// <summary>
    /// Represents the next action in a request pipeline.
    /// </summary>
    /// <typeparam name="TResponse">The response type.</typeparam>
    /// <returns>A task that represents the asynchronous operation, containing the response.</returns>
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();

    public interface IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IEither
    {
        Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
    }
}