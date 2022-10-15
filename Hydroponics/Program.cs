using Asp.Versioning;
using Asp.Versioning.Builder;
using HealthChecks.UI.Client;
using Hydroponics.Controllers.Requests;
using Hydroponics.Data;
using Hydroponics.Filters;
using Hydroponics.Model;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

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
builder.Services.AddApiVersioning(options =>
{
  options.DefaultApiVersion = new ApiVersion(1, 0);
  options.ReportApiVersions = true;
  options.AssumeDefaultVersionWhenUnspecified = true;
});

ApiVersion currentVersion = new ApiVersion(1, 0);

// b. swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
  setup.OperationFilter<ApiVersionOperationFilter>();
  setup.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = builder.Configuration["Swagger:Version"],
    Title = builder.Configuration["Swagger:Title"],
    Description = builder.Configuration["Swagger:Description"],
    Contact = new OpenApiContact
    {
      Name = builder.Configuration["Swagger:Name"],
      Email = builder.Configuration["Swagger:Email"],
      Url = new Uri(builder.Configuration["Swagger:GitHub"] ?? ""),
    }
  });

  // Set the comments path for the Swagger JSON and UI.
  string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
  setup.IncludeXmlComments(xmlPath, includeControllerXmlComments: false);
  setup.EnableAnnotations();
});
builder.Services.AddProblemDetails();

// c. db configuration
builder.Services.AddDbContext<HydroponicsContext>(options =>
{
  string azureSql = builder.Configuration.GetConnectionString("AzureSqlAuth") ?? "";
  options.UseSqlServer(azureSql);
  options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

// d. health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<HydroponicsContext>();

// e. health checks UI
// Registers required services for health checks
builder.Services.AddHealthChecksUI(setupSettings: setup =>
{
  setup.SetEvaluationTimeInSeconds(int.Parse(builder.Configuration["HealthChecks:EvaluationTimeInSeconds"]??""));
  setup.AddHealthCheckEndpoint("Azure SQL Server", "/health");
})
  .AddInMemoryStorage();

WebApplication app = builder.Build();

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

app.UseHttpsRedirection();

//app.UsePathBase(new PathString("/api"));

app.UseRouting();

app.UseEndpoints(endpoints =>
  endpoints.MapHealthChecks("/health", new HealthCheckOptions
  {
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
  }));

// define app version set
ApiVersionSet versionSet = app.NewApiVersionSet()
                    .HasApiVersion(currentVersion)
                    .ReportApiVersions()
                    .Build();

app.MapGet("/cultivationmethods", async (HydroponicsContext db) =>
  await db.CultivationMethods.ToListAsync()
  is List<CultivationMethod> cm ? Results.Ok(cm) : Results.NotFound())
.Produces<List<CultivationMethod>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetCultivationMethods")
.WithTags(new[] { "ups" })
.WithOpenApi()
.WithApiVersionSet(versionSet)
.HasApiVersion(currentVersion);

app.MapPost("/cultivationmethods", (HydroponicsContext db, [FromBody] CultivationMethodRequest body) =>
{
  throw new NotImplementedException("not yey");
})
//.Accepts<CultivationMethodRequest>("application/json")
.Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
.Produces<List<CultivationMethod>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("NewCultivationMethod")
.WithTags(new[] { "ups" })
.WithOpenApi()
.WithApiVersionSet(versionSet)
.HasApiVersion(currentVersion);

app.MapGet("/measures", async (HydroponicsContext db) =>
  await db.Measures.ToListAsync()
  is List<Measure> measures ? Results.Ok(measures) : Results.NotFound())
.Produces<List<Measure>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetMeasures")
.WithTags(new[] { "puka" })
.WithOpenApi()
.WithApiVersionSet(versionSet)
.HasApiVersion(currentVersion);

app.MapGet("/pots", async (HydroponicsContext db) =>
  await db.Pots.ToListAsync()
  is List<Pot> pots ? Results.Ok(pots) : Results.NotFound())
.Produces<List<Pot>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetPots")
.WithOpenApi()
.WithApiVersionSet(versionSet)
.HasApiVersion(currentVersion);

app.MapGet("/substrates", async (HydroponicsContext db) =>
  await db.Substrates.ToListAsync()
  is List<Substrate> subs ? Results.Ok(subs) : Results.NotFound())
.Produces<List<Substrate>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetSubstrates")
.WithOpenApi()
.WithApiVersionSet(versionSet)
.HasApiVersion(currentVersion);

// set health checks UI path
//app.UseHealthChecksUI(config => config.UIPath = "/hc-ui");

app.Run();