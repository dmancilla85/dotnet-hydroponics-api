using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace TestApiEndpoints.Helpers
{
  internal static class TestHelpers
  {
    private const string JsonMediaType = "application/json";
    private const int ExpectedMaxElapsedMilliseconds = 5000;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    internal record AccessAllowed(string Token);
    internal record User(string UserName, string Password);

    public static async Task<string> GetToken(HttpClient client)
    {
      User data = new("test-user", "P@ssword");
      HttpResponseMessage response = client.PostAsJsonAsync("api/v1/access", data).Result;
      string result = await response.Content.ReadAsStringAsync();
      JsonSerializerOptions options = new()
      {
        PropertyNameCaseInsensitive = true
      };

      AccessAllowed? access = JsonSerializer.Deserialize<AccessAllowed>(result, options);

      if (access == null)
      {
        return "";
      }

      return access.Token;
    }

    internal static async Task AssertResponseWithContentAsync<T>(Stopwatch stopwatch,
    HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode,
    T expectedContent)
    {
      AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
      Assert.Equal(JsonMediaType, response.Content.Headers.ContentType?.MediaType);
      Assert.Equal(expectedContent, await JsonSerializer.DeserializeAsync<T?>(
          await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions));
    }

    internal static async Task AssertResponseWithContainingContentAsync<T>(Stopwatch stopwatch,
        HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode,
        T expectedContent)
    {
      AssertCommonResponseParts(stopwatch, response, expectedStatusCode);
      Assert.Equal(JsonMediaType, response.Content.Headers.ContentType?.MediaType);
      IEnumerable<T>? actualContent = await JsonSerializer.DeserializeAsync<IEnumerable<T>>(
          await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions);
      
        Assert.True(actualContent?.Contains(expectedContent));
    }

    internal static void AssertCommonResponseParts(Stopwatch stopwatch,
            HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
    {
      Assert.Equal(expectedStatusCode, response.StatusCode);
      Assert.True(stopwatch.ElapsedMilliseconds < ExpectedMaxElapsedMilliseconds);
    }
    public static StringContent GetJsonStringContent<T>(T model)
        => new(JsonSerializer.Serialize(model), Encoding.UTF8, JsonMediaType);
  }
}
