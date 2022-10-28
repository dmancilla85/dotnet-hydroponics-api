using Hydroponics.Data;
using Microsoft.EntityFrameworkCore;

namespace Hydroponics.Extensions;

internal static class DatabaseExtensions
{
    public static void AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddDbContext<HydroponicsContext>(options =>
        {
            string azureSql = configuration.GetConnectionString("AzureSqlAuth") ?? "";
            options.UseSqlServer(azureSql);
            options.EnableSensitiveDataLogging(env.IsDevelopment());
        });

        // health checks
        services.AddHealthChecks()
                .AddDbContextCheck<HydroponicsContext>();
    }
}
