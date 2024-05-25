using Hydroponics.Api.Infrastructure.Services;
using Hydroponics.Data;
using Hydroponics.Tests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Hydroponics.Tests.Fixtures;

public class CustomWebApplicationFactory<TProgram>
  : WebApplicationFactory<TProgram> where TProgram : class
{
    public DateTime CurrentTestDate { get; set; }
    private readonly DbContextOptions<HydroponicsContext> _contextOptions;

    public CustomWebApplicationFactory()
    {
        _contextOptions = new DbContextOptionsBuilder<HydroponicsContext>()
            .UseInMemoryDatabase("HydroponicsTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        using var context = new HydroponicsContext(_contextOptions);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.AddRange(new PotsFixture().GetAll());
        context.AddRange(new CultivationMethodsFixture().GetAll());
        context.AddRange(new MeasuresFixture().GetAll());
        context.AddRange(new SubstratesFixture().GetAll());
        context.SaveChanges();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(services =>
        {
            ServiceDescriptor? service = services.SingleOrDefault(d => d.ServiceType == typeof(IMetricsService));

            List<ServiceDescriptor> list = [];

            if (service != null)
            {
                list.Add(service);
            }

            foreach (var item in list)
            {
                services.Remove(item);
            }

            services.AddSingleton<IMetricsService, MetricsServiceMock>();

            var context = services.SingleOrDefault(d => d.ServiceType == typeof(HydroponicsContext));

            if (context != null)
            {
                services.Remove(context);
            }

            services.AddDbContext<HydroponicsContext>(opt => _contextOptions.BuildOptionsFragment());
        });
    }
}
