using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using static Hydroponics.Tests.Helpers.TestHelpers;

namespace Hydroponics.Tests.IntegrationTests;

public class TestSubstrates : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    private bool NeedsBearerToken() => _httpClient.DefaultRequestHeaders.Authorization == null;

    private async Task SetBearerToken()
    {
        string token = await GetToken(_httpClient);
        _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
    }

    public TestSubstrates(WebApplicationFactory<Program> webApplicationFactory)
    {
        _ = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));

        _httpClient = webApplicationFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "600");
    }

    internal record Substrate(int Id, string Name);

    [Trait("Category", "Substrates")]
    [Fact(DisplayName = "When calling GET /substrates then the API returns OK")]
    public async Task WhenCallingGetSubstrates_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        // TODO: edit
        List<Substrate> expectedContent =
        [
          new Substrate(1, "COCO COIR"),
      new Substrate(2, "SPONGE"),
      new Substrate(3, "CLAY PEBBLES"),
      new Substrate(4, "PLASTIC PELLETS"),
      new Substrate(5, "PEAT PELLET"),
      new Substrate(6,  "SPHAGNUM")
          ];
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        var response = await _httpClient.GetAsync("api/v1/substrates");

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact]
    public async Task WhenCallingGetSubstratesByID_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        // TODO: edit
        Substrate expectedContent = new(2, "SPONGE");
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        var response = await _httpClient.GetAsync("api/v1/substrates/2");

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact]
    public async Task WhenCallingUpdateSubstratesByID_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;
        // TODO: edit
        Substrate elementToUpdate = new(2, "SPONGE");
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        HttpResponseMessage response = await _httpClient.PutAsync("api/v1/substrates/2", GetJsonStringContent(elementToUpdate));

        // Assert.
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
    }
}
