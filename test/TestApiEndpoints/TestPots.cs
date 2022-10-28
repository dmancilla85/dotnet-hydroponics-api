using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using static TestApiEndpoints.Helpers.TestHelpers;

namespace TestApiEndpoints;

public class TestPots : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    private bool NeedsBearerToken() => _httpClient.DefaultRequestHeaders.Authorization == null;

    private async Task SetBearerToken()
    {
        string token = await GetToken(_httpClient);
        _httpClient.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
    }

    public TestMTestPotseasures(WebApplicationFactory<Program> webApplicationFactory)
    {
        _ = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));

        _httpClient = webApplicationFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "600");
    }

    internal record Sample(int Id, string Name);

    [Fact]
    public async Task WhenCallingGetPots_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        var expectedStatusCode = HttpStatusCode.OK;
        // TODO: edit
        var expectedContent = new List<Sample>()
    {
        new Sample(1, "WICK SYSTEM"),
        new Sample(2, "DWC")
      };
        var stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        var response = _httpClient.GetAsync("api/v1/pots").Result;

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact]
    public async Task WhenCallingGetPotsByID_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        var expectedStatusCode = HttpStatusCode.OK;
        // TODO: edit
        var expectedContent = new Sample(2, "DWC");
        var stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        var response = _httpClient.GetAsync("api/v1/pots/2").Result;

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact]
    public async Task WhenCallingUpdatePotsByID_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        var expectedStatusCode = HttpStatusCode.NoContent;
        // TODO: edit
        var elementToUpdate = new Sample(2, "DWC");
        var stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        var response = await _httpClient.PutAsync("api/v1/pots/2", GetJsonStringContent(elementToUpdate));

        // Assert.
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
    }
}
