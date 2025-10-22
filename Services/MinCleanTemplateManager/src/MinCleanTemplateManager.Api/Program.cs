
using GlobalConstants;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MinCleanTemplateManager.Api;
using MinCleanTemplateManager.Api.Util;
using MinCleanTemplateManager.Application;
using MinCleanTemplateManager.Infrastructure;
using Serilog;
using Serilog.Sinks.OpenTelemetry;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.Extensions.Options;
using MinCleanTemplateManager.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);
StructuredLoggingTemplate.ConfigureStructuredLogging(builder);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{

    var otlpEndpoint = builder.Configuration["Otlp:Endpoint"] ?? OTLPParams.EndPoint;
    loggerConfiguration
        .ReadFrom.Configuration(hostingContext.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = otlpEndpoint; // OpenTelemetry Collector endpoint
            options.Protocol = OtlpProtocol.Grpc;
            options.IncludedData = IncludedData.TraceIdField | IncludedData.SpanIdField;
            options.ResourceAttributes = new Dictionary<string, object>
            {
                { "service.name", builder.Configuration["Otlp:ServiceName"] ??Assembly.GetExecutingAssembly().GetName().Name },
                { "service.version", builder.Configuration["Otlp:Version"] ??"0.0.0" },
                { "host.name", Environment.MachineName },
                { "environment", builder.Environment.EnvironmentName },

            };
        });
});

#if (EnableSwaggerSupport)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerAuthConfigOptions>();
#endif

builder.Services.AddSignalR();
builder.Services.AddAPIServices(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});


var app = builder.Build();
app.UseStatusCodePages();

#if (EnableSwaggerSupport)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endif
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.RegisterMinCleanTemplateManagerEndpoints();
app.UseCors(CorsConstants.Cors_Policy);
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

if (app.Environment.IsDevelopment())
{
    await app.SeedMinCleanTemplateManagerTestingDataAsync();

}
else
{
    await app.SeedMinCleanTemplateManagerRealDataAsync();
}

app.Run();