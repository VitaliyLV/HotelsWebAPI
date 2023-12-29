using Microsoft.Extensions.Diagnostics.HealthChecks;

public class HotelsHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        bool isHealthy = true;
        //todo
        if (isHealthy)
        {
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus));
    }
}
