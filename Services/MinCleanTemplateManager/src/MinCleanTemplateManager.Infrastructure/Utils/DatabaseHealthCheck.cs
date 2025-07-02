using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MinCleanTemplateManager.Infrastructure.Persistence;

namespace MinCleanTemplateManager.Infrastructure.Utils
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly MinCleanTemplateManagerContext _MinCleanTemplateManagerContext;
        public DatabaseHealthCheck(MinCleanTemplateManagerContext minCleanTemplateManagerContext)
        {
            _MinCleanTemplateManagerContext = minCleanTemplateManagerContext;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(
       HealthCheckContext context,
       CancellationToken cancellationToken = default)
        {
            try
            {
                // 1. Get the database name from the DbContext.
                var dbName = _MinCleanTemplateManagerContext.Database.GetDbConnection().Database;

                // 2. Create a dictionary to hold your custom data.
                var data = new Dictionary<string, object>
                {
                    { "DatabaseName", dbName }
                };

                // 3. Attempt to connect to the database.
                var canConnect = await _MinCleanTemplateManagerContext.Database.CanConnectAsync(cancellationToken);

                if (canConnect)
                {
                    // 4. Return a healthy result with the custom data.
                    return HealthCheckResult.Healthy(
                        description: $"Successfully connected to the database: {dbName}",
                        data: data);
                }

                // If CanConnectAsync returns false, it's an unhealthy state.
                return HealthCheckResult.Unhealthy(
                    description: "Could not connect to the database.",
                    data: data);
            }
            catch (Exception ex)
            {
                // If an exception is thrown, return an unhealthy result with the error.
                return HealthCheckResult.Unhealthy(
                    description: "An exception occurred while checking the database.",
                    exception: ex);
            }
        }
        //public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        //{
        //    var isHealthy = _ProductManagerContext.Database.CanConnect();
        //    return Task.FromResult(isHealthy ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy());
        //}
    }
}
