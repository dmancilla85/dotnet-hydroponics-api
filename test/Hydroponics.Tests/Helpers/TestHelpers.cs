using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Text.Json;

namespace Hydroponics.Tests.Helpers;

internal static class TestHelpers
{
    public const string BSON_DATE_FORMAT = "yyyy-MM-ddTHH:mm:ss.FFFZ";
    private const string JsonMediaType = "application/json";
    private const int ExpectedMaxElapsedMilliseconds = 50000;
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    internal record AccessAllowed(string Token);
    internal record User(string UserName, string Password);

    internal static async Task<string> GetToken(HttpClient client)
    {
        User data = new("test-user", "P@ssword");
        var response = client.PostAsJsonAsync("api/v1/access", data).Result;
        var result = await response.Content.ReadAsStringAsync();
        JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        var access = JsonSerializer.Deserialize<AccessAllowed>(result, options);

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
        var actualContent = await JsonSerializer.DeserializeAsync<IEnumerable<T>>(
            await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions);

        Assert.True(actualContent?.Contains(expectedContent));
    }

    internal static void AssertCommonResponseParts(Stopwatch stopwatch,
            HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
    {
        Assert.Equal(expectedStatusCode, response.StatusCode);
        Assert.True(stopwatch.ElapsedMilliseconds < ExpectedMaxElapsedMilliseconds);
    }

    internal static StringContent GetJsonStringContent<T>(T model)
        => new(JsonSerializer.Serialize(model), Encoding.UTF8, JsonMediaType);

    [SupportedOSPlatform("windows6.1")]
    internal static string GenerateBase64ImageString()
    {
        if (!OperatingSystem.IsWindows())
        {
            return "THIS IS NOT AN IMAGE :D"; // no image
        }

        // 1. Create a bitmap
        using Bitmap bitmap = new(80, 20, PixelFormat.Format24bppRgb);
        // 2. Get access to the raw bitmap data
        var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

        // 3. Generate RGB noise and write it to the bitmap's buffer. Note that we are assuming that
        // data.Stride == 3 * data.Width for simplicity/brevity here.
        var noise = new byte[data.Width * data.Height * 3];
        new Random().NextBytes(noise);
        Marshal.Copy(noise, 0, data.Scan0, noise.Length);

        bitmap.UnlockBits(data);

        // 4. Save as JPEG and convert to Base64
        using MemoryStream jpegStream = new();
        bitmap.Save(jpegStream, ImageFormat.Jpeg);
        return Convert.ToBase64String(jpegStream.ToArray());
    }

    [SupportedOSPlatform("windows6.1")]
    internal static string GenerateImageString()
    {
        if (!OperatingSystem.IsWindows()) //
        {
            return "THIS IS NOT AN IMAGE :O"; // no image
        }

        // 1. Create a bitmap
        using Bitmap bitmap = new(80, 20, PixelFormat.Format24bppRgb);
        // 2. Get access to the raw bitmap data
        var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

        // 3. Generate RGB noise and write it to the bitmap's buffer. Note that we are assuming that
        // data.Stride == 3 * data.Width for simplicity/brevity here.
        var noise = new byte[data.Width * data.Height * 3];
        new Random().NextBytes(noise);
        Marshal.Copy(noise, 0, data.Scan0, noise.Length);

        bitmap.UnlockBits(data);

        // 4. Save as JPEG and convert to Base64
        using MemoryStream jpegStream = new();
        bitmap.Save(jpegStream, ImageFormat.Jpeg);

        byte[] stream = jpegStream.ToArray();
        string? result = Convert.ToString(stream);

        return result ?? "";
    }
}
