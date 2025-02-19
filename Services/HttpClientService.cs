using System.Text;
using System.Text.Json;

namespace ContentGenerator.Services;

class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _gemeniHttpClient;
    public HttpClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _gemeniHttpClient = _httpClientFactory.CreateClient("Gemeni");
    }
    public async Task<string> GemeniGetReq(string text, int inputLength)
    {
        string reqBody = $"{{\"contents\": [{{ \"parts\":[{{\"text\": \"{text}\"}}]}}] }}";
        object obj = new {
            key = "1",
            value = "2"
        };
        string json = JsonSerializer.Serialize(obj);
        var content = new StringContent(reqBody, Encoding.UTF8, "application/json");
        var response = await _gemeniHttpClient.PostAsync(_gemeniHttpClient.BaseAddress, content);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // Handle the error
            return "";
        }
    }
}