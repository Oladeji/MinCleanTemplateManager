﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace CQRSHelper
{
    public static class CQRSMediatr
    {
        public static IServiceCollection AddMediater(this IServiceCollection services, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            // Register all handlers in the assembly

            services.AddScoped<ISender, Sender>();

            // Register all handlers in the assembly
            var handleInterfaceType = typeof(IRequestHandler<,>);
            var handlerTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && !type.IsInterface)
                .SelectMany(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handleInterfaceType)
                    .Select(i => new { HandlerImplemenation = type, Interface = i }));

            foreach (var handler in handlerTypes)
            {
                var handlerType = handler.HandlerImplemenation;
                var interfaceType = handler.Interface;
                // Register the handler with the DI container                services.AddScoped(interfaceType, handlerType);
            }

            return services;
        }
    }
}
