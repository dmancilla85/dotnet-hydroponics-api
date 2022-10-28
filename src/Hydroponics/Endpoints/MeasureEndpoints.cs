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

internal static class MeasureEndpoints
{
    private static readonly string Collection = "measures";

    public static async Task<IResult> Create([FromServices] HydroponicsContext db,
        [FromServices] IResilienceService polly,
        [FromServices] IMapper mapper, NewMeasure element)
    {
        Measure entity = mapper.Map<Measure>(element);

        await polly.CircuitBreakerWithRetry(async () =>
        {
            await db.Measures.AddAsync(entity);
            await db.SaveChangesAsync();
        });

        var result = mapper.Map<ViewMeasure>(entity);
        return TypedResults.Created($"/{Collection}/{result.Id}", result);
    }

    public static async Task<IResult> Delete([FromServices] HydroponicsContext db,
        [FromServices] ILogger<Program> logger,
        [FromServices] IResilienceService polly,
        [FromServices] IMetricsService metrics,
        int id)
    {
        if (await db.Measures.FindAsync(id) is Measure element)
        {
            await polly.CircuitBreakerWithRetry(async () =>
            {
                db.Measures.Remove(element);
                await db.SaveChangesAsync();
            });
            return Results.NoContent();
        }

        string message = $"There is no measure registered with ID {id}";
        metrics.CountWarning($"{Collection}-{MethodBase.GetCurrentMethod()?.Name}");
        LogWriter.LogBadRequest(logger, message);

        return Results
            .Problem(message, statusCode:
                    StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> Get([FromServices] HydroponicsContext db,
      [FromServices] IMapper mapper, int id)
    {
        return await db.Measures.FindAsync(id) is Measure element
                ? Results.Ok(mapper.Map<ViewMeasure>(element))
                : Results.Problem($"There is no measure registered with ID {id}",
                        statusCode: StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> List([FromServices] HydroponicsContext db,
       [FromServices] IMapper mapper) => await db.Measures.ToListAsync()
    is List<Measure> list ?
        Results.Ok(mapper.Map<IEnumerable<ViewMeasure>>(list)) :
        Results.Problem("There are no measures registered",
            statusCode: StatusCodes.Status404NotFound);

    public static void MapMeasuresEndpoints(this WebApplication app, string basePath, ApiVersionSet versionSet,
                                        ApiVersion currentVersion)
    {
        app.MapGet($"{basePath}/{Collection}", List)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces<IEnumerable<ViewMeasure>>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithOpenApi()
          .WithName("ListMeasures")
          .WithTags(new[] { Collection })
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapGet($"{basePath}/{Collection}/{{id}}", Get)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces<ViewMeasure>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithOpenApi()
          .WithName("GetMeasureById")
          .WithTags(new[] { Collection })
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapPost($"{basePath}/{Collection}", Create)
          .RequireAuthorization()
          .AddEndpointFilter<ValidationFilter<NewMeasure>>()
          .RequireRateLimiting("sliding")
          .Accepts<NewMeasure>("application/json")
          .Produces<ViewMeasure>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithOpenApi()
          .WithName("CreateMeasure")
          .WithTags(new[] { Collection })
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapPut($"{basePath}/{Collection}/{{id}}", Update)
          .RequireAuthorization()
         .RequireRateLimiting("sliding")
         .AddEndpointFilter<ValidationFilter<EditMeasure>>()
         .Accepts<EditMeasure>("application/json")
         .Produces(StatusCodes.Status204NoContent)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .ProducesProblem(StatusCodes.Status500InternalServerError)
         .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
         .WithOpenApi()
         .WithName("UpdateMeasure")
         .WithTags(new[] { Collection })
         .WithApiVersionSet(versionSet)
         .HasApiVersion(currentVersion);

        app.MapDelete($"{basePath}/{Collection}/{{id}}", Delete)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithOpenApi()
          .WithName("DeleteMeasure")
          .WithTags(new[] { Collection })
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);
    }

    public static async Task<IResult> Update([FromServices] HydroponicsContext db,
        [FromServices] IResilienceService polly,
        [FromServices] ILogger<Program> logger,
        [FromServices] IMetricsService metrics,
        [FromServices] IMapper mapper,
        int id,
        EditMeasure updatedElement)
    {
        Measure? entity = await db.Measures.FindAsync(id);

        if (entity is null)
        {
            string message = $"There is no measure registered with ID {id}";
            metrics.CountWarning($"{Collection}-{MethodBase.GetCurrentMethod()?.Name}");
            LogWriter.LogBadRequest(logger, message);
            return Results.Problem(message, statusCode: StatusCodes.Status404NotFound);
        }

        mapper.Map(updatedElement, entity);

        await polly.CircuitBreakerWithRetry(async () =>
        {
            await db.SaveChangesAsync();
        });

        return Results.NoContent();
    }
}
