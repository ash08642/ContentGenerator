using System.Text;
using System.Text.Json;

namespace ContentGenerator.Services;

class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _gemeniHttpClient;
    private readonly HttpClient _murfHttpClient;
    private readonly ILogger<HttpClientService> _logger;
    public HttpClientService(IHttpClientFactory httpClientFactory, ILogger<HttpClientService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _gemeniHttpClient = _httpClientFactory.CreateClient("Gemeni");
        _murfHttpClient = _httpClientFactory.CreateClient("Murf");
        _logger = logger;
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

    public async Task<string> MurfGetReq(string text)
    {
        var body = new
        {
            voiceId                 = "de-DE-matthias",
            style                   = "Conversational",
            text                    = text,
            rate                    = 0,
            pitch                   = 0,
            sampleRate              = 48000,
            format                  = "MP3",
            channelType             = "MONO",
            pronunciationDictionary = new {},
            encodeAsBase64          = false,
            variation               = 1,
            audioDuration           = 0,
            modelVersion            = "GEN2",
            multiNativeLocale      = "de-DE"
        };
        string json = JsonSerializer.Serialize(body);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //content.Headers.Add("Content-Type", "application/json");
        //content.Headers.Add("Accept", "application/json");
        content.Headers.Add("api-key", "ap2_ce598101-0202-4200-8525-2fba36f20e7f");

        var response = await _murfHttpClient.PostAsync(_murfHttpClient.BaseAddress, content);
        _logger.LogInformation("Logging Murf Http");
        _logger.LogInformation(await response.Content.ReadAsStringAsync());
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