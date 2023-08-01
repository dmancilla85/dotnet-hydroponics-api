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

internal static class PotEndpoints
{
    private static readonly string Collection = "pots";

    public static async Task<IResult> Create([FromServices] HydroponicsContext db,
        [FromServices] IResilienceService polly,
        [FromServices] IMapper mapper,
        NewPot element)
    {
        Pot entity = mapper.Map<Pot>(element);

        await polly.CircuitBreakerWithRetry(async () =>
        {
            await db.Pots.AddAsync(entity);
            await db.SaveChangesAsync();
        });

        var result = mapper.Map<ViewPot>(entity);
        return TypedResults.Created($"/{Collection}/{result.Id}", result);
    }

    public static async Task<IResult> Delete([FromServices] HydroponicsContext db,
        [FromServices] ILogger<Program> logger,
        [FromServices] IResilienceService polly,
        [FromServices] IMetricsService metrics,
        int id)
    {
        if (await db.Pots.FindAsync(id) is Pot element)
        {
            await polly.CircuitBreakerWithRetry(async () =>
            {
                db.Pots.Remove(element);
                await db.SaveChangesAsync();
            });
            return Results.NoContent();
        }

        string message = $"There is no pot registered with ID {id}";
        metrics.CountWarning($"{Collection}-{MethodBase.GetCurrentMethod()?.Name}");
        LogWriter.LogBadRequest(logger, message);

        return Results
            .Problem(message, statusCode:
                    StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> Get([FromServices] HydroponicsContext db,
      [FromServices] IMapper mapper, int id)
    {
        return await db.Pots.FindAsync(id) is Pot element
                ? Results.Ok(mapper.Map<ViewPot>(element))
                : Results.Problem($"There is no pot registered with ID {id}",
                        statusCode: StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> List([FromServices] HydroponicsContext db,
      [FromServices] IMapper mapper) => await db.Pots.ToListAsync()
                    is List<Pot> list ?
        Results.Ok(mapper.Map<IEnumerable<ViewPot>>(list)) :
        Results.Problem("There are no pots registered",
            statusCode: StatusCodes.Status404NotFound);

    public static void MapPotsEndpoints(this WebApplication app, string basePath, ApiVersionSet versionSet,
                                        ApiVersion currentVersion)
    {
        app.MapGet($"{basePath}/{Collection}", List)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces<IEnumerable<ViewPot>>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithOpenApi()
          .WithName("ListPots")
          .WithTags( Collection )
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapGet($"{basePath}/{Collection}/{{id}}", Get)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces<ViewPot>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .WithOpenApi()
          .WithName("GetPotById")
          .WithTags(Collection )
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapPost($"{basePath}/{Collection}", Create)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .AddEndpointFilter<ValidationFilter<NewPot>>()
          .Accepts<NewPot>("application/json")
          .Produces<ViewPot>(StatusCodes.Status201Created)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithOpenApi()
          .WithName("CreatePot")
          .WithTags( Collection )
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapPut($"{basePath}/{Collection}/{{id}}", Update)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .AddEndpointFilter<ValidationFilter<EditPot>>()
          .Accepts<EditPot>("application/json")
          .Produces(StatusCodes.Status204NoContent)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithOpenApi()
          .WithName("UpdatePot")
          .WithTags( Collection )
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);

        app.MapDelete($"{basePath}/{Collection}/{{id}}", Delete)
          .RequireAuthorization()
          .RequireRateLimiting("sliding")
          .Produces(StatusCodes.Status204NoContent)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status500InternalServerError)
          .ProducesProblem(StatusCodes.Status503ServiceUnavailable)
          .WithName("DeletePot")
          .WithTags( Collection )
          .WithOpenApi()
          .WithApiVersionSet(versionSet)
          .HasApiVersion(currentVersion);
    }

    public static async Task<IResult> Update([FromServices] HydroponicsContext db,
        [FromServices] IResilienceService polly,
        [FromServices] ILogger<Program> logger,
        [FromServices] IMetricsService metrics,
        [FromServices] IMapper mapper,
        int id,
        EditPot updatedElement)
    {
        Pot? entity = await db.Pots.FindAsync(id);

        if (entity is null)
        {
            string message = $"There is no pot registered with ID {id}";
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
