using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace GlobalConstants
{
    /// <summary>
    /// Template for structured logging configuration across all services
    /// This template provides basic structured logging without external dependencies
    /// </summary>
    public static class StructuredLoggingTemplate
    {
        /// <summary>
        /// Configures basic structured logging for the application
        /// </summary>
        public static void ConfigureStructuredLogging(WebApplicationBuilder builder)
        {
            // Configure basic logging without Serilog dependencies
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            
            // Set logging levels
            builder.Logging.SetMinimumLevel(LogLevel.Information);
            builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
            builder.Logging.AddFilter("Microsoft.Hosting.Lifetime", LogLevel.Information);
            builder.Logging.AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Information);
            builder.Logging.AddFilter("System", LogLevel.Warning);
            
            // Filter out noisy health check logs from request logging middleware
            // This reduces log volume while preserving important error information
            builder.Logging.AddFilter("GlobalConstants.RequestLoggingMiddleware", (logLevel) =>
            {
                // Always log warnings and errors (including health check failures)
                return logLevel >= LogLevel.Warning;
            });
        }

        /// <summary>
        /// Configures basic request logging middleware with health check filtering
        /// </summary>
        public static void ConfigureRequestLogging(WebApplication app)
        {
            // Basic request logging middleware with health check filtering
            app.Use(async (context, next) =>
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<RequestLoggingMiddleware>>();
                var startTime = DateTime.UtcNow;
                var stopwatch = Stopwatch.StartNew();

                // Skip logging for health check endpoints (except errors)
                var isHealthCheck = context.Request.Path.StartsWithSegments("/health");

                try
                {
                    // Only log request start for non-health check endpoints
                    if (!isHealthCheck)
                    {
                        logger.LogInformation(
                            "HTTP {RequestMethod} {RequestPath} started at {StartTime} from {RemoteIpAddress}",
                            context.Request.Method,
                            context.Request.Path,
                            startTime,
                            context.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
                    }

                    await next();

                    stopwatch.Stop();

                    // Determine log level based on response status
                    var logLevel = context.Response.StatusCode >= 500 ? LogLevel.Error :
                                  context.Response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;

                    // Log response for non-health checks or for health check errors/warnings
                    if (!isHealthCheck || logLevel >= LogLevel.Warning)
                    {
                        logger.Log(logLevel,
                            "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedMilliseconds}ms",
                            context.Request.Method,
                            context.Request.Path,
                            context.Response.StatusCode,
                            stopwatch.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    
                    // Always log errors, even for health checks
                    logger.LogError(ex,
                        "HTTP {RequestMethod} {RequestPath} failed after {ElapsedMilliseconds}ms with error: {ErrorMessage}",
                        context.Request.Method,
                        context.Request.Path,
                        stopwatch.ElapsedMilliseconds,
                        ex.Message);
                    throw;
                }
            });
        }

        /// <summary>
        /// Logs service startup information with structured data
        /// </summary>
        public static void LogServiceStartup(WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILogger<ServiceStartup>>();
            var serviceName = app.Configuration["Otlp:ServiceName"] ?? Assembly.GetExecutingAssembly().GetName().Name;
            var serviceVersion = app.Configuration["Otlp:Version"] ?? Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";

            logger.LogInformation(
                "Service {ServiceName} v{ServiceVersion} starting in {Environment} at {StartupTime}. " +
                "MachineName: {MachineName}, ProcessId: {ProcessId}, DotNetVersion: {DotNetVersion}",
                serviceName,
                serviceVersion,
                app.Environment.EnvironmentName,
                DateTime.UtcNow,
                Environment.MachineName,
                Environment.ProcessId,
                Environment.Version.ToString());

            // Log configuration values (be careful with sensitive data)
            var configKeys = new[] { "Otlp:Endpoint", "Otlp:ServiceName" };
            foreach (var key in configKeys)
            {
                var value = app.Configuration[key];
                if (!string.IsNullOrEmpty(value))
                {
                    bool isSensitive = key.Contains("ConnectionString", StringComparison.OrdinalIgnoreCase) ||
                                     key.Contains("Password", StringComparison.OrdinalIgnoreCase) ||
                                     key.Contains("Secret", StringComparison.OrdinalIgnoreCase);
                    
                    var displayValue = isSensitive ? "***REDACTED***" : value;
                    logger.LogInformation("Configuration {ConfigKey} = {ConfigValue}", key, displayValue);
                }
            }
        }

        /// <summary>
        /// Configures health check endpoints - use this only for services WITHOUT Health Checks UI
        /// For services with Health Checks UI, configure health checks manually with UIResponseWriter
        /// </summary>
        public static void ConfigureBasicHealthCheckEndpoints(WebApplication app)
        {
            // Basic health check endpoint without Health Checks UI dependency
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = new
                    {
                        status = report.Status.ToString(),
                        timestamp = DateTime.UtcNow,
                        totalDuration = report.TotalDuration.TotalMilliseconds,
                        entries = report.Entries.ToDictionary(
                            entry => entry.Key,
                            entry => new
                            {
                                status = entry.Value.Status.ToString(),
                                description = entry.Value.Description,
                                duration = entry.Value.Duration.TotalMilliseconds,
                                data = entry.Value.Data?.ToDictionary(d => d.Key, d => d.Value?.ToString())
                            })
                    };
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
                }
            });

            // Simplified health check for load balancers
            app.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("ready"),
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = new
                    {
                        status = report.Status.ToString(),
                        timestamp = DateTime.UtcNow,
                        totalDuration = report.TotalDuration.TotalMilliseconds
                    };
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
                }
            });

            // Live check for basic service responsiveness
            app.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = _ => false,
                ResponseWriter = async (context, _) =>
                {
                    context.Response.ContentType = "application/json";
                    var result = new
                    {
                        status = "Healthy",
                        timestamp = DateTime.UtcNow,
                        service = Assembly.GetExecutingAssembly().GetName().Name
                    };
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
                }
            });
        }

        /// <summary>
        /// Backwards compatibility - calls ConfigureBasicHealthCheckEndpoints
        /// </summary>
        [Obsolete("Use ConfigureBasicHealthCheckEndpoints for services without Health Checks UI, or configure health checks manually for services with Health Checks UI")]
        public static void ConfigureHealthCheckEndpoints(WebApplication app)
        {
            //ConfigureBasicHealthCheckEndpoints(app);
        }
    }

    /// <summary>
    /// Marker classes for logger generic types
    /// </summary>
    public class RequestLoggingMiddleware { }
    public class ServiceStartup { }

    /// <summary>
    /// Extension methods for service-specific logging
    /// </summary>
    public static class ServiceLoggingExtensions
    {
        public static void LogServiceInitialization<T>(this ILogger<T> logger, string serviceName, string version)
        {
            logger.LogInformation(
                "Service {ServiceName} v{Version} initialized in {Environment} at {Timestamp}",
                serviceName,
                version,
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown",
                DateTime.UtcNow);
        }

        public static void LogServiceShutdown<T>(this ILogger<T> logger, string serviceName)
        {
            logger.LogInformation(
                "Service {ServiceName} shutting down at {Timestamp}. Uptime: {Uptime}",
                serviceName,
                DateTime.UtcNow,
                DateTime.UtcNow - Process.GetCurrentProcess().StartTime);
        }

        public static void LogConfigurationLoaded<T>(this ILogger<T> logger, IConfiguration configuration)
        {
            logger.LogInformation(
                "Configuration loaded at {Timestamp}",
                DateTime.UtcNow);
        }

        public static void LogDependencyRegistration<T>(this ILogger<T> logger, string dependencyName, string version = null)
        {
            logger.LogInformation(
                "Dependency {DependencyName} {Version} registered at {Timestamp}",
                dependencyName,
                version ?? "Unknown",
                DateTime.UtcNow);
        }
    }
}