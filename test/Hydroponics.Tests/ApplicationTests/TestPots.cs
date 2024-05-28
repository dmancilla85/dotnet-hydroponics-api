using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Hydroponics.Data.Entities;
using Hydroponics.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using static Hydroponics.Tests.Helpers.TestHelpers;

namespace Hydroponics.Tests.IntegrationTests;

public class TestPots : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private const string Category = "Application tests for pots";
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

    [Trait("Category", Category)]
    [Fact(DisplayName = "Get all pots")]
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

        var response = await _httpClient.GetAsync(TestRoutes.POTS);

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Trait("Category", Category)]
    [Fact(DisplayName = "Get pot by ID")]
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

        var response = await _httpClient.GetAsync($"{TestRoutes.POTS}/2");

        // Assert.
        await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
    }

    [Trait("Category", Category)]
    [Fact(DisplayName = "Update pot by ID")]
    public async Task WhenCallingUpdatePotsByID_ThenTheAPIReturnsExpectedResponse()
    {
        // Arrange.
        HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;
        // TODO: edit
        Pot elementToUpdate = new()
        {
            Name = "WASP2",
            Height = 0.45m,
            Length = 0.34m,
            Width = 0.26m,
            Liters = 20.0m,
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        // Act.
        if (NeedsBearerToken())
        {
            await SetBearerToken();
        }

        HttpResponseMessage response = await _httpClient.PutAsync($"{TestRoutes.POTS}/2", GetJsonStringContent(elementToUpdate));
        // Assert.
        AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
    }
}
