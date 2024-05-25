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

internal static class SubstrateEndpoints
{
    private static readonly string Collection = "substrates";

    public static async Task<IResult> Create([FromServices] HydroponicsContext db,
        [FromServices] IResilienceService polly,
        [FromServices] IMapper mapper,
        NewSubstrate element)
    {
        Substrate entity = mapper.Map<Substrate>(element);
        await polly.CircuitBreakerWithRetry(async () =>
        {
            await db.Substrates.AddAsync(entity);
            await db.SaveChangesAsync();
        });

        var result = mapper.Map<ViewSubstrate>(entity);
        return TypedResults.Created($"/{Collection}/{result.Id}", result);
    }

    public static async Task<IResult> Delete([FromServices] HydroponicsContext db,
        [FromServices] ILogger<Program> logger,
        [FromServices] IResilienceService polly,
        [FromServices] IMetricsService metrics,
        int id)
    {
        if (await db.Substrates.FindAsync(id) is Substrate element)
        {
            await polly.CircuitBreakerWithRetry(async () =>
            {
                db.Substrates.Remove(element);
                await db.SaveChangesAsync();
            });
            return Results.NoContent();
        }

        string message = $"There is no substrate registered with ID {id}";
        metrics.CountWarning($"{Collection}-{MethodBase.GetCurrentMethod()?.Name}");
        LogWriter.LogBadRequest(logger, message);

        return Results
            .Problem(message, statusCode:
                    StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> Get([FromServices] HydroponicsContext db,
      [FromServices] IMapper mapper, int id)
    {
        return await db.Substrates.FindAsync(id) is Substrate element
                ? Results.Ok(mapper.Map<ViewSubstrate>(element))
                : Results.Problem($"There is no substrate registered with ID {id}",
                        statusCode: StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> List([FromServices] HydroponicsContext db,
      [FromServices] IMapper mapper) => await db.Substrates.ToListAsync()
                    is List<Substrate> list ?
        Results.Ok(mapper.Map<IEnumerable<ViewSubstrate>>(list)) :
        Results.Problem("There are no substrates registered",
            statusCode: StatusCodes.Status404NotFound);

    public static void MapSubstratesEndpoints(this WebApplication app, string basePath, ApiVersionSet versionSet,
                                        ApiVersion currentVersion)
    {
        app.MapGet($"{basePath}/{Collection}", List)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces<IEnumerable<ViewSubstrate>>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithOpenApi()
          .WithName("ListSubstrates")
          .WithTags(Collection)
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapGet($"{basePath}/{Collection}/{{id}}", Get)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces<ViewSubstrate>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithOpenApi()
          .WithName("GetSubstrateById")
          .WithTags(Collection)
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapPost($"{basePath}/{Collection}", Create)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .AddEndpointFilter<ValidationFilter<NewSubstrate>>()
          .Accepts<NewSubstrate>("application/json")
          .Produces<ViewSubstrate>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithOpenApi()
          .WithName("CreateSubstrate")
          .WithTags(Collection)
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapPut($"{basePath}/{Collection}/{{id}}", Update)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .AddEndpointFilter<ValidationFilter<EditSubstrate>>()
          .Accepts<EditSubstrate>("application/json")
          .Produces(StatusCodes.Status204NoContent)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithOpenApi()
          .WithName("UpdateSubstrate")
          .WithTags(Collection)
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
          .WithName("DeleteSubstrate")
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
        EditSubstrate updatedElement)
    {
        Substrate? entity = await db.Substrates.FindAsync(id);

        if (entity is null)
        {
            string message = $"There is no substrate registered with ID {id}";
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
