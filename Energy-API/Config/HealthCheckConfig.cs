using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Energy_API.Config
{
    public static class HealthCheckConfig
    {
        // Método de extensão para configurar os HealthChecks
        public static void ConfigureHealthChecks(this IServiceCollection services, string mongoConnectionString)
        {
            if (string.IsNullOrEmpty(mongoConnectionString))
            {
                throw new ArgumentNullException("MongoDB connection string cannot be null or empty.");
            }

            // Configura o HealthCheck para MongoDB e o status básico do servidor
            services.AddHealthChecks()
                    .AddMongoDb(mongoConnectionString, name: "MongoDB", timeout: TimeSpan.FromSeconds(3))
                    .AddCheck("Server", () => HealthCheckResult.Healthy("Server is up and running"));
        }

        // Método para fornecer a resposta JSON detalhada
        public static HealthCheckOptions JsonResponseWriter()
        {
            return new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var json = System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        results = report.Entries.Select(e => new
                        {
                            name = e.Key,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description,
                            duration = e.Value.Duration
                        })
                    });

                    await context.Response.WriteAsync(json);
                }
            };
        }
    }
}

