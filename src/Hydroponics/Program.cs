using Asp.Versioning;
using Asp.Versioning.Builder;
using HealthChecks.UI.Client;
using Hydroponics.Api.Infrastructure.Options;
using Hydroponics.Api.Infrastructure.Services;
using Hydroponics.Endpoints;
using Hydroponics.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// appsettings and logger setup
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Configuration.AddYamlFile("appsettings.yml", optional: false)
  .AddYamlFile($"appsettings.{builder.Environment.EnvironmentName}.yml", optional: false)
  .AddUserSecrets<Program>();

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();

// Add services to the container.
// a. versioning
ApiVersion currentVersion = new(1, 0);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = currentVersion;
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
});

string basePath = $"api/v{currentVersion.MajorVersion}";

// problem details
builder.Services.AddProblemDetails();

// authentication
builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();

// c. swagger
builder.Services.AddSwaggerConfiguration(builder.Configuration);

// d. validators, services and options
builder.Services.AddValidators();
builder.Services.Configure<ResilienceOptions>(builder.Configuration.GetSection(ResilienceOptions.SectionName));
builder.Services.AddSingleton<IMetricsService, MetricsService>();
builder.Services.AddSingleton<IResilienceService, ResilienceService>();
builder.Services.AddAutoMapper(/*AppDomain.CurrentDomain.GetAssemblies()*/typeof(Program));

// d. db configuration
builder.Services.AddDatabaseConfiguration(builder.Configuration, builder.Environment);

// f. health checks UI
// Registers required services for health checks (now only for prod)
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddHealthChecksUI(setupSettings: setup =>
    {
        setup.SetEvaluationTimeInSeconds(int.Parse(builder.Configuration["HealthChecks:EvaluationTimeInSeconds"] ?? ""));
        setup.AddHealthCheckEndpoint("Azure SQL Server", "/health");
    })
    .AddInMemoryStorage();
}

// g. rate limiter
builder.Services.AddRateLimiterConfiguration(builder.Configuration);

WebApplication app = builder.Build();

// define app version set
ApiVersionSet versionSet = app.NewApiVersionSet()
  .HasApiVersion(currentVersion)
  .ReportApiVersions()
  .Build();

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayOperationId();
        c.DisplayRequestDuration();
    });
}
else
{
    // set health checks UI path
    app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");
}

app.UseRouting();
app.UseAuthorization();

app.MapMetrics("api/metrics");
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// middlewares
app.UseResilienceMiddleware();

// endpoints
app.MapAccessEndpoints(basePath, versionSet, currentVersion);
app.MapCultivationMethodsEndpoints(basePath, versionSet, currentVersion);
app.MapPotsEndpoints(basePath, versionSet, currentVersion);
app.MapSubstratesEndpoints(basePath, versionSet, currentVersion);
app.MapMeasuresEndpoints(basePath, versionSet, currentVersion);

app.UseHttpsRedirection();

app.Run();

public partial class Program { }