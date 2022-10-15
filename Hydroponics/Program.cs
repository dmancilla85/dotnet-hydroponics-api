using HealthChecks.UI.Client;
using Hydroponics.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Configuration.AddYamlFile("appsettings.yml", optional: false)
  .AddYamlFile($"appsettings.{builder.Environment.EnvironmentName}.yml", optional: false)
  .AddUserSecrets<Program>();

Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration)
              .CreateLogger();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

// db configuration
builder.Services.AddDbContext<HydroponicsContext>(options =>
{
  string azureSql = builder.Configuration.GetConnectionString("AzureSqlAuth") ?? "";
  options.UseSqlServer(azureSql);
  options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<HydroponicsContext>();

// Registers required services for health checks
builder.Services.AddHealthChecksUI(setupSettings: setup => setup
   .AddHealthCheckEndpoint("Azure SQL Server", "/api/health"))
  .AddInMemoryStorage();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseEndpoints(endpoints => 
  endpoints.MapHealthChecks("api/health", new HealthCheckOptions
  {
    Predicate= _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
  }));

app.MapGet("/substrates", (HydroponicsContext db) => db.Substrates.ToListAsync())
.WithName("GetSubstrates")
.WithOpenApi();

app.MapGet("/cultivationmethods", (HydroponicsContext db) => db.CultivationMethods.ToListAsync())
.WithName("GetCultivationMethods")
.WithOpenApi();

app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

app.Run();