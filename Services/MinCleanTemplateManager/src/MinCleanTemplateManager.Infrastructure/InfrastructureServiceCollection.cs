using Domain.DBContext;
using Domain.Interfaces;
using GlobalConstants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinCleanTemplateManager.Infrastructure.Persistence;
using MinCleanTemplateManager.Infrastructure.Utils;



namespace MinCleanTemplateManager.Infrastructure
{

    public static class InfrastructureServiceCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IWebHostEnvironment environment)
        {
            var applicationAssembly = typeof(InfrastructureServiceCollection).Assembly;


         
            var connectionName = GlobalConstants.APINames.MinCleanTemplateManagerAPI + "Conn";
            var constr = RepositoryHelper.EncryptionHelper.GetAppConnectionString(connectionName, configuration, "NOTPRODUCTION");
           
            var connectionStringProvider = new GlobalConstants.ConnectionStringProvider { ConnectionString = constr };
            services.AddSingleton(connectionStringProvider);
            #if (UseSqlServer)
                        services.AddDbContext<MinCleanTemplateManagerContext>(options =>
                        {
                            options.UseSqlServer(constr);
                            options.EnableSensitiveDataLogging();
                            options.LogTo(Console.WriteLine, LogLevel.Information);
                        });
            #elif (UsePostgreSql)
                        services.AddDbContext<MinCleanTemplateManagerContext>(options => options.UseNpgsql(constr));
            #elif (UseSqlite)
                        services.AddDbContext<MinCleanTemplateManagerContext>(options => options.UseSqlite(constr));
            #else
                        services.AddDbContext<MinCleanTemplateManagerContext>(option => option.UseMySql(constr, GeneralUtils.GetMySqlVersion()));
            #endif
            RepositoryHelper.RepositoryRegistration.RegisterRepositories(services, applicationAssembly);

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<MinCleanTemplateManagerContext>));

            services.AddHealthChecks().AddCheck<DatabaseHealthCheck>("Database");
        //    services.AddInstrumentation(configuration);
            return services;
        }


        //private static IServiceCollection AddInstrumentation(this IServiceCollection services, IConfiguration configuration)
        //{

        //    string serviceName = configuration["Otlp:ServiceName"] ?? Assembly.GetExecutingAssembly().GetName().Name;
        //    string version = configuration["Otlp:Version"] ?? GlobalConstants.OTLPParams.Version;
        //    var otlpEndpoint = new Uri(configuration["Otlp:Endpoint"] ?? GlobalConstants.OTLPParams.EndPoint);

        //    services.AddOpenTelemetry()
        //        .ConfigureResource(resource =>
        //        {
        //            resource
        //            .AddService(serviceName)
        //            .AddAttributes(new List<KeyValuePair<string, object>>()
        //            {
        //            new KeyValuePair<string, object>("service.version", Assembly.GetExecutingAssembly().GetName().Version!.ToString()),

        //            });
        //        })

        //        .WithTracing(tracerProviderBuilder =>
        //        {
        //            tracerProviderBuilder
        //                .AddSource(serviceName + ".Tracing")
        //                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        //                .AddAspNetCoreInstrumentation()
        //                .AddHttpClientInstrumentation()
        //                    #if UseSqlServer
        //                                            .AddSqlClientInstrumentation()
        //                    #elif UsePostgreSql
        //                                            .AddNpgsql()
        //                    #elif UseMySql
        //                                            .AddConnectorNet()
        //                    #endif
        //                                    .AddEntityFrameworkCoreInstrumentation(options =>
        //                                    {
        //                                        options.SetDbStatementForText = true;
        //                                        options.EnrichWithIDbCommand = (activity, command) =>
        //                                        {
        //                                            if (command is DbCommand dbCommand)
        //                                            {
        //                                                activity.SetTag("db.statement", dbCommand.CommandText);
        //                                                activity.SetTag("db.command_type", dbCommand.CommandType.ToString());
        //                                                activity.SetTag("db.parameters", string.Join(", ", dbCommand.Parameters.Cast<DbParameter>().Select(p => $"{p.ParameterName}={p.Value}")));
        //                                            }
        //                                        };
        //                                    })
        //                .AddConsoleExporter()
        //                .AddOtlpExporter(options =>
        //                {
        //                    options.Endpoint = otlpEndpoint;

        //                });

        //        })


        //        .WithMetrics(meterProviderBuilder =>
        //        {
        //            meterProviderBuilder
        //           .AddMeter(

        //                      "Microsoft.AspNetCore.Hosting",
        //                      "Microsoft.AspNetCore.Server.Kestrel",
        //                      "System.Net.Http",
        //                      "System.Runtime",
        //                      "System.Threading.Tasks")

        //                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
        //                .AddAspNetCoreInstrumentation()
        //                .AddHttpClientInstrumentation()
        //                .AddRuntimeInstrumentation()
        //                .AddProcessInstrumentation()
        //                .AddOtlpExporter(options =>
        //                {
        //                    options.Endpoint = otlpEndpoint;
        //                });
        //        });


        //    return services;


        //}


    }

}
