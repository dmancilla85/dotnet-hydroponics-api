using System.Threading.RateLimiting;
using Hydroponics.Api.Infrastructure.Options;
using Microsoft.AspNetCore.RateLimiting;

namespace Hydroponics.Extensions
{
    internal static class RateLimiterExtensions
    {
        public static void AddRateLimiterConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            RateLimitingOptions rateLimitOptions = new();
            configuration.GetSection(RateLimitingOptions.SectionName).Bind(rateLimitOptions);
            string slidingPolicy = "sliding";

            services.AddRateLimiter(_ => _.AddSlidingWindowLimiter(
              policyName: slidingPolicy, options =>
              {
                  options.PermitLimit = rateLimitOptions.PermitLimit;
                  options.Window = TimeSpan.FromSeconds(rateLimitOptions.Window);
                  options.SegmentsPerWindow = rateLimitOptions.SegmentsPerWindow;
                  options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                  options.QueueLimit = rateLimitOptions.QueueLimit;
              }));
        }
    }
}
