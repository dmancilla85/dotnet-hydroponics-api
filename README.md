---
title: "Hydroponics API"
author: "David A. Mancilla"
tags: ["net7","aspnetcore", "sqlserver", "apirest","swagger","openapi",
"problemdetails","http2","prometheus-metrics","logging","jwt","authentication",
"authorization","healthchecks","circuitbreaker","retry","docker","cors","auto-mapper","dto",
"docker-compose","environments","user-secrets","minimal-api","api-versioning"]
---

# Hydroponics API

## BACKLOG
* [ ] Stress test on rate limiters
* [ ] Domain Drive Design / structure the project correctly

## TODO

## DONE
* [x] Simple authentication
* [x] Authorization
* [x] API Versioning
* [x] Azure SQL Server
* [x] DefineMessage / Logger
* [x] Entity Framework Core
* [x] Metrics
* [x] Minimal API
* [x] NET 7 Rate Limiter
* [x] Swagger
* [x] Options Pattern
* [x] Polly CircuitBreaker / Retry
* [x] ProblemDetails
* [x] Serilog (*) : Agregar a todos los endpoints
* [x] Sonar Analyzer
* [x] Validator pattern
* [x] XUnit
* [x] YML Configuration

## DEV NOTES

### Filters
Filters allow you to run code at certain stages of the request processing pipeline. In other words, a filter is a piece of code that is executed before or after an action method is executed. For example, you could use a filter to log every time someone accesses a web page or to validate the request parameters sent to an endpoint.

### API Versioning
This API uses the Asp.Versioning package. It requires to add the service with AddApiVersioning, defining a default API version. It is neccesary to add a VersionSet to each endpoint using the method WithApiVersionSet.
Also, it adds the required configuration in the Swagger doc using the ApiVersionOperationFilter.

### High Performance Logging
The LoggerMessage class exposes functionality to create cacheable delegates that require fewer object allocations and reduced computational overhead compared to logger extension methods, such as LogInformation and LogDebug. For high-performance logging scenarios, use the LoggerMessage pattern.

### Rate Limiter
A sliding window algorithm:
* Is similar to the fixed window limiter but adds segments per window. The window slides one segment each segment interval. The segment interval is (window time)/(segments per window).
* Limits the requests for a window to permitLimit requests.
* Each time window is divided in n segments per window.
* Requests taken from the expired time segment one window back (n segments prior to the current segment), are added to the current segment. 
	We refer to the most expired time segment one window back as the expired segment.
