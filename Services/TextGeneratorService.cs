
namespace ContentGenerator.Services;

public class TextGeneratorService : ITextGeneratorService
{
    private readonly IHttpClientService _httpClientService;
    public TextGeneratorService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }
    public async Task<string?> GenerateText(string query)
    {
        return await _httpClientService.GemeniGetReq("Tell me about Cristiano Ronaldo", 100);
    }
}