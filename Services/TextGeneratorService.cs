
using System.Text.Json.Nodes;

namespace ContentGenerator.Services;

public class TextGeneratorService : ITextGeneratorService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ILogger<TextGeneratorService> _logger;
    public TextGeneratorService(IHttpClientService httpClientService, ILogger<TextGeneratorService> logger)
    {
        _httpClientService = httpClientService;
        _logger = logger;
    }
    public async Task<string?> GenerateText(string query)
    {
        string rawResponse = await _httpClientService.GemeniGetReq(query, 100);
        _logger.LogInformation(rawResponse);
        JsonNode gemeniNode = JsonNode.Parse(rawResponse)!;
        //_logger.LogInformation(gemeniNode["candidates"]![0]!["content"]!["parts"]![0]!["text"]!.ToString());
        return gemeniNode["candidates"]![0]!["content"]!["parts"]![0]!["text"]!.ToString();
    }
}