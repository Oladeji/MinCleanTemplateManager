﻿
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using CQRSHelper;

namespace MinCleanTemplateManager.Application;

public static class ApplicationServiceCollection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {

        var applicationAssembly = typeof(ApplicationServiceCollection).Assembly;


        services.AddMediater(applicationAssembly);

       // services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReferenceMarker>();
        return services;
    }
}
