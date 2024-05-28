using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Hydroponics.Data.Entities;
using Hydroponics.Tests.Fixtures;
using Hydroponics.Tests.Helpers;
using static Hydroponics.Tests.Helpers.TestHelpers;

namespace Hydroponics.Tests.IntegrationTests;

public class TestSubstratesInMemory : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private const string Category = "Integration tests with SQL in-memory for pots";
    private readonly HttpClient _httpClient;

    private bool NeedsBearerToken() => _httpClient.DefaultRequestHeaders.Authorization == null;

    private async Task SetBearerToken()
    {
        string token = await GetToken(_httpClient);
        _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
    }

    public TestSubstratesInMemory(CustomWebApplicationFactory<Program> webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "600");
    }

    [Trait("Category", Category)]
    [Fact(DisplayName = "Get all substrates")]
    public async Task GetSubstrates_OK()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        Stopwatch stopwatch = Stopwatch.StartNew();

        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        // Act.
        HttpResponseMessage response = await _httpClient.GetAsync(TestRoutes.SUBSTRATES);

        var res = await response.Content.ReadAsStringAsync();

        // Assert.
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
    }

    [Trait("Category", Category)]
    [Fact(DisplayName = "Create a new substrate")]
    public async Task CreateSubstrate_OK()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
        // TODO: edit
        Substrate elementToCreate = new()
        {
            Name = "BLOTTING PAPER"
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        HttpResponseMessage response = await _httpClient.PostAsync(TestRoutes.SUBSTRATES, GetJsonStringContent(elementToCreate));
        
        var res = await response.Content.ReadAsStringAsync();

        // Assert.
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
    }
}
