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
              
                var dbName = _MinCleanTemplateManagerContext.Database.GetDbConnection().Database;

             
                var data = new Dictionary<string, object>
                {
                    { "DatabaseName", dbName }
                };

               
                var canConnect = await _MinCleanTemplateManagerContext.Database.CanConnectAsync(cancellationToken);

                if (canConnect)
                {
                   
                    return HealthCheckResult.Healthy(
                        description: $"Successfully connected to the database: {dbName}",
                        data: data);
                }

             
                return HealthCheckResult.Unhealthy(
                    description: "Could not connect to the database.",
                    data: data);
            }
            catch (Exception ex)
            {
              
                return HealthCheckResult.Unhealthy(
                    description: "An exception occurred while checking the database.",
                    exception: ex);
            }
        }

    }
}
