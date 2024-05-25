using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Hydroponics.Tests.Fixtures;
using Hydroponics.Tests.Helpers;
using static Hydroponics.Tests.Helpers.TestHelpers;

namespace Hydroponics.Tests.IntegrationTests;

public class TestPotsInMemory : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string Category = "Pruebas integrales";
    private readonly HttpClient _httpClient;

    private bool NeedsBearerToken() => _httpClient.DefaultRequestHeaders.Authorization == null;

    private async Task SetBearerToken()
    {
        string token = await GetToken(_httpClient);
        _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
    }

    public TestPotsInMemory(CustomWebApplicationFactory<Program> webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "600");
    }

    [Trait("Category", Category)]
    [Fact(DisplayName = "Get pots.")]
    public async Task GetPots_OK()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        Stopwatch stopwatch = Stopwatch.StartNew();

        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        // Act.
        HttpResponseMessage response = await _httpClient.GetAsync($"{TestRoutes.POTS}");

        // Assert.
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
    }
}
