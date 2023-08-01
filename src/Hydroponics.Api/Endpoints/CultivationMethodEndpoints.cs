using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.Builder;
using AutoMapper;
using Hydroponics.Api.Data.DataTransferObjects;
using Hydroponics.Api.Infrastructure.Filters;
using Hydroponics.Api.Infrastructure.Logging;
using Hydroponics.Api.Infrastructure.Services;
using Hydroponics.Data;
using Hydroponics.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hydroponics.Endpoints;

internal static class CultivationMethodEndpoints
{
    private static readonly string Collection = "cultivationmethods";

    public static async Task<IResult> Create([FromServices] HydroponicsContext db,
        [FromServices] IResilienceService polly, [FromServices] IMapper mapper,
        NewCultivationMethod element)
    {
        CultivationMethod entity = mapper.Map<CultivationMethod>(element);

        await polly.CircuitBreakerWithRetry(async () =>
            {
                await db.CultivationMethods.AddAsync(entity);
                await db.SaveChangesAsync();
            });

        var result = mapper.Map<ViewCultivationMethod>(entity);

        return TypedResults.Created($"/{Collection}/{result.Id}", result);
    }

    public static async Task<IResult> Delete([FromServices] HydroponicsContext db,
        [FromServices] ILogger<Program> logger,
        [FromServices] IResilienceService polly,
        [FromServices] IMetricsService metrics,
        int id)
    {
        if (await db.CultivationMethods.FindAsync(id) is CultivationMethod element)
        {
            await polly.CircuitBreakerWithRetry(async () =>
            {
                db.CultivationMethods.Remove(element);
                await db.SaveChangesAsync();
            });
            return Results.NoContent();
        }

        string message = $"There is no cultivation method registered with ID {id}";
        metrics.CountWarning($"{Collection}-{MethodBase.GetCurrentMethod()?.Name}");
        LogWriter.LogBadRequest(logger, message);

        return Results.Problem(message, statusCode:
                    StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> Get([FromServices] HydroponicsContext db,
      [FromServices] IMapper mapper, int id)
    {
        return await db.CultivationMethods.FindAsync(id) is CultivationMethod element
                ? Results.Ok(mapper.Map<ViewCultivationMethod>(element))
                : Results.Problem($"There is no cultivation method registered with ID {id}",
                        statusCode: StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> List([FromServices] HydroponicsContext db,
      [FromServices] IMapper mapper) => await db.CultivationMethods.ToListAsync()
                    is List<CultivationMethod> cm ?
        Results.Ok(mapper.Map<IEnumerable<ViewCultivationMethod>>(cm)) :
        Results.Problem("There are no cultivation methods registered",
            statusCode: StatusCodes.Status404NotFound);

    public static void MapCultivationMethodsEndpoints(this WebApplication app, string basePath, ApiVersionSet versionSet,
                                        ApiVersion currentVersion)
    {
        app.MapGet($"{basePath}/{Collection}", List)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces<IEnumerable<ViewCultivationMethod>>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithOpenApi()
          .WithName("ListCultivationMethods")
          .WithTags(Collection)
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapGet($"{basePath}/{Collection}/{{id}}", Get)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces<ViewCultivationMethod>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithOpenApi()
          .WithName("GetCultivationMethodById")
          .WithTags(Collection)
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapPost($"{basePath}/{Collection}", Create)
          .RequireRateLimiting("sliding")
          .AddEndpointFilter<ValidationFilter<NewCultivationMethod>>()
          .Accepts<NewCultivationMethod>("application/json")
          .Produces<ViewCultivationMethod>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithOpenApi()
          .WithName("CreateCultivationMethod")
          .WithTags( Collection )
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapPut($"{basePath}/{Collection}/{{id}}", Update)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .AddEndpointFilter<ValidationFilter<EditCultivationMethod>>()
          .Accepts<EditCultivationMethod>("application/json")
          .Produces(StatusCodes.Status204NoContent)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithOpenApi()
          .WithName("UpdateCultivationMethod")
          .WithTags(Collection)
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapDelete($"{basePath}/{Collection}/{{id}}", Delete)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces(StatusCodes.Status204NoContent)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithOpenApi()
          .WithName("DeleteCultivationMethod")
          .WithTags(Collection)
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);
    }

    public static async Task<IResult> Update([FromServices] HydroponicsContext db,
        [FromServices] IResilienceService polly,
        [FromServices] ILogger<Program> logger,
        [FromServices] IMetricsService metrics,
        [FromServices] IMapper mapper,
        int id,
        EditCultivationMethod updatedElement)
    {
        CultivationMethod? entity = await db.CultivationMethods.FindAsync(id);

        if (entity is null)
        {
            string message = $"There is no cultivation method registered with ID {id}";
            metrics.CountWarning($"{Collection}-{MethodBase.GetCurrentMethod()?.Name}");
            LogWriter.LogBadRequest(logger, message);
            return Results.Problem(message, statusCode: StatusCodes.Status404NotFound);
        }

        mapper.Map(updatedElement, entity);

        await polly.CircuitBreakerWithRetry(async () => await db.SaveChangesAsync());

        return Results.NoContent();
    }
}
