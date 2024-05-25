using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using static Hydroponics.Tests.Helpers.TestHelpers;

namespace Hydroponics.Tests.IntegrationTests;

public class TestCultivationMethods : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    private bool NeedsBearerToken() => _httpClient.DefaultRequestHeaders.Authorization == null;

    private async Task SetBearerToken()
    {
        string token = await GetToken(_httpClient);
        _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
    }

    public TestCultivationMethods(WebApplicationFactory<Program> webApplicationFactory)
    {
        _ = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));

        _httpClient = webApplicationFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "600");
    }

    internal record CultivationMethod(int Id, string Name);

    [Trait("Category", "Cultivation Methods")]
    [Fact(DisplayName = "When calling GetCultivationMethods, then the API returns OK")]
    public async Task WhenCallingGetCultivationMethods_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        List<CultivationMethod> expectedContent =
        [
            new CultivationMethod(1, "WICK SYSTEM"),
        new CultivationMethod(2, "DWC"),
        new CultivationMethod(3, "RDWC"),
        new CultivationMethod(4, "DRIP SYSTEM"),
        new CultivationMethod(5, "NUTRIENT FILM")
          ];
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        var response = await _httpClient.GetAsync("api/v1/cultivationmethods");

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Trait("Category", "Cultivation Methods")]
    [Fact]
    public async Task WhenCallingGetCultivationMethodsByID_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        CultivationMethod expectedContent = new(2, "DWC");
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        HttpResponseMessage response = await _httpClient.GetAsync("api/v1/cultivationmethods/2");

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Trait("Category", "Cultivation Methods")]
    [Fact]
    public async Task WhenCallingUpdateCultivationMethodsByID_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;
        CultivationMethod elementToUpdate = new(2, "DWC");
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        HttpResponseMessage response = await _httpClient
                .PutAsync("api/v1/cultivationmethods/2", GetJsonStringContent(elementToUpdate));

        // Assert.
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
    }
}
