
using System.Text;
using System.Text.Json.Nodes;

namespace ContentGenerator.Services;

public class GemeniTextGeneratorService : ITextGeneratorService
{
    private readonly ILogger<GemeniTextGeneratorService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _gemeniHttpClient;
    public GemeniTextGeneratorService(ILogger<GemeniTextGeneratorService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _gemeniHttpClient = _httpClientFactory.CreateClient("Gemeni");

    }
    public async Task<string> GenerateText(string query)
    {
        string rawResponse = await GemeniGetReq(query, 100);
        _logger.LogInformation(rawResponse);
        JsonNode gemeniNode = JsonNode.Parse(rawResponse)!;
        //_logger.LogInformation(gemeniNode["candidates"]![0]!["content"]!["parts"]![0]!["text"]!.ToString());
        return gemeniNode["candidates"]![0]!["content"]!["parts"]![0]!["text"]!.ToString();
    }

    public async Task<string> GemeniGetReq(string text, int inputLength)
    {
        string reqBody = $"{{\"contents\": [{{ \"parts\":[{{\"text\": \"{text}\"}}]}}] }}";
        /*object obj = new {
            key = "1",
            value = "2"
        };
        string json = JsonSerializer.Serialize(obj);*/
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