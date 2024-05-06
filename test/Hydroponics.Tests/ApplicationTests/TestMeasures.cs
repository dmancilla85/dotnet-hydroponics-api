using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Testing;
using static TestApiEndpoints.Helpers.TestHelpers;

namespace Hydroponics.Tests.IntegrationTests;

public class TestMeasures : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly HttpClient _httpClient;

  private bool NeedsBearerToken() => _httpClient.DefaultRequestHeaders.Authorization == null;

  private async Task SetBearerToken()
  {
    string token = await GetToken(_httpClient);
    _httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
  }

  public TestMeasures(WebApplicationFactory<Program> webApplicationFactory)
  {
    _ = webApplicationFactory ?? throw new ArgumentNullException(nameof(webApplicationFactory));

    _httpClient = webApplicationFactory.CreateClient();
    _httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
    _httpClient.DefaultRequestHeaders.Add("Keep-Alive", "600");
  }

  internal record Measure(int Id, string Name, string Description, string Units, decimal MinValue, decimal MaxValue);

  [Fact]
  public async Task WhenCallingGetMeasures_ThenTheAPIReturnsExpectedResponse()
  {
    // Arrange.
    HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
    // TODO: edit
    List<Measure> expectedContent =
    [
      new Measure(1,"TEMPERATURE","Temperature in degrees",  "°C" ,0.00m ,50.00m),
      new Measure(2,"PH","Potential of Hydrogen","",   -1.00m, 15.00m),
      new Measure(3,"EC","Electrical Conductivity","S/m-1",0.00m ,1.00m),
      new Measure(4,"HUMIDITY","Humidity in the environment","%",0.00m,100.00m)
    ];
    Stopwatch stopwatch = Stopwatch.StartNew();

    // Act.
    if (NeedsBearerToken())
    {
      await SetBearerToken();
    }

    var response = await _httpClient.GetAsync("api/v1/measures");

    // Assert.
    await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
  }

  [Fact]
  public async Task WhenCallingGetMeasuresByID_ThenTheAPIReturnsExpectedResponse()
  {
    // Arrange.
    HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
    // TODO: edit
    Measure expectedContent = new(2, "PH", "Potential of Hydrogen", "", -1.00m, 15.00m);
    Stopwatch stopwatch = Stopwatch.StartNew();

    // Act.
    if (NeedsBearerToken())
    {
      await SetBearerToken();
    }

    var response = await _httpClient.GetAsync("api/v1/measures/2");

    // Assert.
    await AssertResponseWithContentAsync(stopwatch, response, expectedStatusCode, expectedContent);
  }

  [Fact]
  public async Task WhenCallingUpdateMeasuresByID_ThenTheAPIReturnsExpectedResponse()
  {
    // Arrange.
    HttpStatusCode expectedStatusCode = HttpStatusCode.NoContent;
    // TODO: edit
    Measure elementToUpdate = new(2, "PH", "Potential of Hydrogen", "", -1.00m, 15.00m);
    Stopwatch stopwatch = Stopwatch.StartNew();

    // Act.
    if (NeedsBearerToken())
    {
      await SetBearerToken();
    }

    HttpResponseMessage response = await _httpClient.PutAsync("api/v1/measures/2", GetJsonStringContent(elementToUpdate));

    // Assert.
    AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
  }
}
