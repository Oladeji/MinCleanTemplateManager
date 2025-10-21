namespace CQRSHelper
{
    //public static class HandlerRegistration
    //{

    //    public static void RegisterHandlers(IServiceCollection services, Assembly assembly)
    //    {
    //        var handlerTypes = assembly.GetTypes()
    //            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Handler"))
    //            .ToList();
    //        foreach (var handlerType in handlerTypes)
    //        {
    //            var interfaceType = handlerType.GetInterfaces().FirstOrDefault(i => i.Name == "I" + handlerType.Name);
    //            if (interfaceType != null)
    //            {
    //                services.AddScoped(interfaceType, handlerType);
    //            }
    //        }
    //    }
    //}


    //public interface IBaseCQRS { }

    ////public interface ICQRSHandler<TOutError, TOutResult, TRequest> where TRequest : IBaseCQRS
    ////{
    ////    Task<Either<TOutError, TOutResult>> Handle(TRequest basecqrs, CancellationToken cancellationToken = default);
    ////}
    ////public interface ICQRSHandler2<TOutError, TOutResult, TRequest> where TRequest : IBaseCQRS
    ////{
    ////    Task<Either<TOutError, TOutResult>> Handle(TRequest basecqrs, CancellationToken cancellationToken = default);
    ////}

    //public interface ICQRSHandler2<TRequest, TResponse>// : IBaseCQRS
    //{
    //    Task<TResponse> Handle(TRequest basecqrs, CancellationToken cancellationToken = default);
    //}
    //public interface ICQRSHandler<TRequest, TLeft, TRight>
    //{
    //    /// <summary>
    //    /// Handles the given request and returns an Either type with a left (failure) or right (success) value.
    //    /// </summary>
    //    /// <param name="request">The request to handle.</param>
    //    /// <param name="cancellationToken">The cancellation token.</param>
    //    /// <returns>An Either type containing a left value (failure) or a right value (success).</returns>
    //    Task<Either<TLeft, TRight>> Handle(TRequest request, CancellationToken cancellationToken = default);
    //}



}
