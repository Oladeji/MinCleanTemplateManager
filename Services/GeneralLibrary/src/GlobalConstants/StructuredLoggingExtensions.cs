using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace GlobalConstants
{
    /// <summary>
    /// Provides structured logging extensions with consistent formatting and context
    /// </summary>
    public static class StructuredLoggingExtensions
    {
        // Request/Response Logging
        public static void LogRequestStarted<T>(this ILogger<T> logger, 
            string requestName, 
            object? requestData = null,
            [CallerMemberName] string? memberName = null,
            [CallerFilePath] string? sourceFilePath = null)
        {
            logger.LogInformation(
                "Request {RequestName} started in {MemberName} from {SourceFile} at {Timestamp}",
                requestName,
                memberName,
                Path.GetFileName(sourceFilePath ?? "Unknown"),
                DateTime.UtcNow);

            if (requestData != null)
            {
                logger.LogDebug(
                    "Request {RequestName} data: {@RequestData}",
                    requestName,
                    requestData);
            }
        }

        public static void LogRequestCompleted<T>(this ILogger<T> logger,
            string requestName,
            TimeSpan duration,
            bool isSuccess = true,
            [CallerMemberName] string? memberName = null)
        {
            if (isSuccess)
            {
                logger.LogInformation(
                    "Request {RequestName} completed successfully in {MemberName}. Duration: {Duration}ms at {Timestamp}",
                    requestName,
                    memberName,
                    duration.TotalMilliseconds,
                    DateTime.UtcNow);
            }
            else
            {
                logger.LogWarning(
                    "Request {RequestName} completed with issues in {MemberName}. Duration: {Duration}ms at {Timestamp}",
                    requestName,
                    memberName,
                    duration.TotalMilliseconds,
                    DateTime.UtcNow);
            }
        }

        public static void LogRequestFailed<T>(this ILogger<T> logger,
            string requestName,
            Exception exception,
            object? requestData = null,
            [CallerMemberName] string? memberName = null,
            [CallerFilePath] string? sourceFilePath = null)
        {
            logger.LogError(exception,
                "Request {RequestName} failed in {MemberName} from {SourceFile} at {Timestamp}. Error: {ErrorMessage}",
                requestName,
                memberName,
                Path.GetFileName(sourceFilePath ?? "Unknown"),
                DateTime.UtcNow,
                exception.Message);

            if (requestData != null)
            {
                logger.LogDebug(
                    "Failed request {RequestName} data: {@RequestData}",
                    requestName,
                    requestData);
            }
        }

        // Business Logic Logging
        public static void LogBusinessEvent<T>(this ILogger<T> logger,
            string eventName,
            object eventData,
            string? userId = null,
            string? correlationId = null)
        {
            logger.LogInformation(
                "Business event {EventName} occurred. User: {UserId}, CorrelationId: {CorrelationId}, Timestamp: {Timestamp}, Data: {@EventData}",
                eventName,
                userId ?? "System",
                correlationId ?? Guid.NewGuid().ToString(),
                DateTime.UtcNow,
                eventData);
        }

        public static void LogDomainError<T>(this ILogger<T> logger,
            string errorCode,
            string errorType,
            string errorDescription,
            object? context = null,
            [CallerMemberName] string? memberName = null)
        {
            logger.LogError(
                "Domain error occurred in {MemberName}. ErrorCode: {ErrorCode}, ErrorType: {ErrorType}, Description: {ErrorDescription}, Timestamp: {Timestamp}",
                memberName,
                errorCode,
                errorType,
                errorDescription,
                DateTime.UtcNow);

            if (context != null)
            {
                logger.LogDebug(
                    "Domain error context for {ErrorCode}: {@Context}",
                    errorCode,
                    context);
            }
        }

        // Database Operations
        public static void LogDatabaseOperation<T>(this ILogger<T> logger,
            string operationType,
            string entityName,
            object? entityId = null,
            TimeSpan? duration = null,
            int? recordsAffected = null)
        {
            logger.LogInformation(
                "Database {OperationType} on {EntityName}. EntityId: {EntityId}, Duration: {Duration}ms, RecordsAffected: {RecordsAffected}, Timestamp: {Timestamp}",
                operationType,
                entityName,
                entityId?.ToString() ?? "Multiple",
                duration?.TotalMilliseconds ?? 0,
                recordsAffected ?? 0,
                DateTime.UtcNow);
        }

        public static void LogDatabaseError<T>(this ILogger<T> logger,
            string operationType,
            string entityName,
            Exception exception,
            object? entityId = null)
        {
            logger.LogError(exception,
                "Database {OperationType} failed on {EntityName}. EntityId: {EntityId}, Error: {ErrorMessage}, Timestamp: {Timestamp}",
                operationType,
                entityName,
                entityId?.ToString() ?? "Unknown",
                exception.Message,
                DateTime.UtcNow);
        }

        // Performance and Metrics
        public static void LogPerformanceMetric<T>(this ILogger<T> logger,
            string metricName,
            double value,
            string? unit = null,
            Dictionary<string, object>? additionalData = null)
        {
            var logData = new Dictionary<string, object>
            {
                ["MetricName"] = metricName,
                ["Value"] = value,
                ["Unit"] = unit ?? "count",
                ["Timestamp"] = DateTime.UtcNow
            };

            if (additionalData != null)
            {
                foreach (var kvp in additionalData)
                {
                    logData[kvp.Key] = kvp.Value;
                }
            }

            logger.LogInformation(
                "Performance metric {MetricName}: {Value} {Unit} at {Timestamp}",
                metricName,
                value,
                unit ?? "count",
                DateTime.UtcNow);

            logger.LogDebug("Performance metric details: {@MetricData}", logData);
        }

        // Security and Authentication
        public static void LogSecurityEvent<T>(this ILogger<T> logger,
            string eventType,
            string? userId = null,
            string? ipAddress = null,
            string? userAgent = null,
            bool isSuccess = true,
            string? additionalInfo = null)
        {
            var logLevel = isSuccess ? LogLevel.Information : LogLevel.Warning;
            var status = isSuccess ? "Success" : "Failed";

            logger.Log(logLevel,
                "Security event {EventType} - {Status}. UserId: {UserId}, IP: {IpAddress}, UserAgent: {UserAgent}, Info: {AdditionalInfo}, Timestamp: {Timestamp}",
                eventType,
                status,
                userId ?? "Anonymous",
                ipAddress ?? "Unknown",
                userAgent ?? "Unknown",
                additionalInfo ?? "None",
                DateTime.UtcNow);
        }

        // External Service Calls
        public static void LogExternalServiceCall<T>(this ILogger<T> logger,
            string serviceName,
            string operation,
            string? endpoint = null,
            TimeSpan? duration = null,
            int? statusCode = null,
            bool isSuccess = true)
        {
            var logLevel = isSuccess ? LogLevel.Information : LogLevel.Warning;

            logger.Log(logLevel,
                "External service call to {ServiceName}.{Operation}. Endpoint: {Endpoint}, Duration: {Duration}ms, StatusCode: {StatusCode}, Success: {IsSuccess}, Timestamp: {Timestamp}",
                serviceName,
                operation,
                endpoint ?? "Unknown",
                duration?.TotalMilliseconds ?? 0,
                statusCode ?? 0,
                isSuccess,
                DateTime.UtcNow);
        }

        public static void LogExternalServiceError<T>(this ILogger<T> logger,
            string serviceName,
            string operation,
            Exception exception,
            string? endpoint = null,
            int? statusCode = null)
        {
            logger.LogError(exception,
                "External service call to {ServiceName}.{Operation} failed. Endpoint: {Endpoint}, StatusCode: {StatusCode}, Error: {ErrorMessage}, Timestamp: {Timestamp}",
                serviceName,
                operation,
                endpoint ?? "Unknown",
                statusCode ?? 0,
                exception.Message,
                DateTime.UtcNow);
        }

        // Instrument and Hardware Logging
        public static void LogInstrumentOperation<T>(this ILogger<T> logger,
            string instrumentName,
            string operation,
            object? result = null,
            TimeSpan? duration = null,
            bool isSuccess = true,
            string? errorMessage = null)
        {
            var logLevel = isSuccess ? LogLevel.Information : LogLevel.Error;

            logger.Log(logLevel,
                "Instrument {InstrumentName} {Operation} - {Status}. Duration: {Duration}ms, Result: {@Result}, Error: {ErrorMessage}, Timestamp: {Timestamp}",
                instrumentName,
                operation,
                isSuccess ? "Success" : "Failed",
                duration?.TotalMilliseconds ?? 0,
                result,
                errorMessage,
                DateTime.UtcNow);
        }

        // Health Check Logging
        public static void LogHealthCheckResult<T>(this ILogger<T> logger,
            string checkName,
            string status,
            TimeSpan duration,
            string? description = null,
            Dictionary<string, object>? data = null)
        {
            var logLevel = status.ToLowerInvariant() switch
            {
                "healthy" => LogLevel.Information,
                "degraded" => LogLevel.Warning,
                "unhealthy" => LogLevel.Error,
                _ => LogLevel.Information
            };

            logger.Log(logLevel,
                "Health check {CheckName} completed with status {Status} in {Duration}ms. Description: {Description}, Timestamp: {Timestamp}",
                checkName,
                status,
                duration.TotalMilliseconds,
                description ?? "None",
                DateTime.UtcNow);

            if (data != null && data.Any())
            {
                logger.LogDebug("Health check {CheckName} data: {@HealthData}", checkName, data);
            }
        }

        // Validation and Error Context
        public static void LogValidationError<T>(this ILogger<T> logger,
            string fieldName,
            object? fieldValue,
            string validationRule,
            string? context = null,
            [CallerMemberName] string? memberName = null)
        {
            logger.LogWarning(
                "Validation failed in {MemberName}. Field: {FieldName}, Value: {FieldValue}, Rule: {ValidationRule}, Context: {Context}, Timestamp: {Timestamp}",
                memberName,
                fieldName,
                fieldValue,
                validationRule,
                context ?? "None",
                DateTime.UtcNow);
        }

        // Configuration and Startup
        public static void LogConfigurationValue<T>(this ILogger<T> logger,
            string configKey,
            object? configValue,
            bool isSensitive = false)
        {
            var displayValue = isSensitive ? "***REDACTED***" : configValue;
            
            logger.LogInformation(
                "Configuration loaded: {ConfigKey} = {ConfigValue}, Timestamp: {Timestamp}",
                configKey,
                displayValue,
                DateTime.UtcNow);
        }

        public static void LogServiceStartup<T>(this ILogger<T> logger,
            string serviceName,
            string version,
            string environment,
            Dictionary<string, object>? startupMetrics = null)
        {
            logger.LogInformation(
                "Service {ServiceName} v{Version} starting in {Environment} environment at {Timestamp}",
                serviceName,
                version,
                environment,
                DateTime.UtcNow);

            if (startupMetrics != null && startupMetrics.Any())
            {
                logger.LogInformation("Startup metrics: {@StartupMetrics}", startupMetrics);
            }
        }
    }

    /// <summary>
    /// Structured logging scope extensions for creating correlated log contexts
    /// </summary>
    public static class LoggingScopeExtensions
    {
        public static IDisposable BeginRequestScope<T>(this ILogger<T> logger,
            string requestName,
            string? correlationId = null,
            string? userId = null)
        {
            var scope = new Dictionary<string, object>
            {
                ["RequestName"] = requestName,
                ["CorrelationId"] = correlationId ?? Guid.NewGuid().ToString(),
                ["UserId"] = userId ?? "System",
                ["RequestStartTime"] = DateTime.UtcNow
            };

            return logger.BeginScope(scope);
        }

        public static IDisposable BeginOperationScope<T>(this ILogger<T> logger,
            string operationName,
            object? operationData = null)
        {
            var scope = new Dictionary<string, object>
            {
                ["OperationName"] = operationName,
                ["OperationId"] = Guid.NewGuid().ToString(),
                ["OperationStartTime"] = DateTime.UtcNow
            };

            if (operationData != null)
            {
                scope["OperationData"] = operationData;
            }

            return logger.BeginScope(scope);
        }
    }
}