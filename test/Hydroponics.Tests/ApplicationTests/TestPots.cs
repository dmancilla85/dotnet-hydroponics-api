using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Hydroponics.Data.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using static Hydroponics.Tests.Helpers.TestHelpers;

namespace Hydroponics.Tests.IntegrationTests;

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

    public TestPots(WebApplicationFactory<Program> webApplicationFactory)
    {
        _ = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));

        _httpClient = webApplicationFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
        _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "600");
    }

    internal record PotRequest(int Id, string Name);

    [Fact]
    public async Task WhenCallingGetPots_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        // TODO: edit
        List<PotRequest> expectedContent =
        [
            new(  1, "WASP1" ),
        new ( 2, "WASP2" ),
        new ( 3, "PIRANHA1" ),
        new ( 4, "PIRANHA2" )
          ];
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        var response = await _httpClient.GetAsync("api/v1/pots");

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact]
    public async Task WhenCallingGetPotsByID_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        // TODO: edit
        PotRequest expectedContent = new(2, "WASP2");
        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        var response = await _httpClient.GetAsync("api/v1/pots/2");

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Fact]
    public async Task WhenCallingUpdatePotsByID_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;
        // TODO: edit
        Pot elementToUpdate = new()
        {
            Name = "DWC",
            Height = 0.45m,
            Length = 0.34m,
            Width = 0.26m,
            Liters = 20,
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        HttpResponseMessage response = await _httpClient.PutAsync("api/v1/pots/2", GetJsonStringContent(elementToUpdate));

        // Assert.
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
    }
}
