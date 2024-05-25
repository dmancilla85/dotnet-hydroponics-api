using System.Reflection;
using Hydroponics.Api.Infrastructure.Filters;
using Microsoft.OpenApi.Models;

namespace Hydroponics.Extensions;

internal static class SwaggerExtensions
{
    public static void AddSwaggerConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        OpenApiSecurityScheme securityScheme = new()
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JSON Web Token based security",
        };

        OpenApiSecurityRequirement securityReq = new() {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
      Array.Empty<string>()
    }
  };

        OpenApiContact contact = new()
        {
            Name = configuration["Swagger:Name"],
            Email = configuration["Swagger:Email"],
            Url = new Uri(configuration["Swagger:GitHub"] ?? "")
        };

        OpenApiLicense license = new()
        {
            Name = "Free License",
            Url = new Uri(configuration["Swagger:GitHub"] ?? "")
        };

        OpenApiInfo info = new()
        {
            Version = configuration["Swagger:Version"],
            Title = configuration["Swagger:Title"],
            Description = configuration["Swagger:Description"],
            TermsOfService = new Uri(configuration["Swagger:LicenseUrl"] ?? ""),
            Contact = contact,
            License = license
        };

        services.AddSwaggerGen(o =>
        {
            o.OperationFilter<ApiVersionOperationFilter>();
            o.SwaggerDoc("v1", info);
            o.AddSecurityDefinition("Bearer", securityScheme);
            o.AddSecurityRequirement(securityReq);

            // Set the comments path for the Swagger JSON and UI.
            string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            o.IncludeXmlComments(xmlPath, includeControllerXmlComments: false);
            o.EnableAnnotations();
        });
    }
}
