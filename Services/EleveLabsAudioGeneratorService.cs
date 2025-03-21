using Azure;
using Azure.Core;
using ContentGenerator.Models;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.OpenApi.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using System.Text.Json;
using NuGet.Packaging.Signing;

namespace ContentGenerator.Services;

public class EleveLabsAudioGeneratorService : IAudioGeneratorService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly HttpClient _ellevenLabsHttpClient;
    public EleveLabsAudioGeneratorService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _ellevenLabsHttpClient = _httpClientFactory.CreateClient("ElevenLabs");
    }

    public async Task<AudioData> GenerateAudio(string query)
    {
        var body = new
        {
            text = query,
            model_id = "eleven_flash_v2_5"
        };
        string json = JsonSerializer.Serialize(body);
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //content.Headers.Add("Content-Type", "application/json");
        content.Headers.Add("xi-api-key", "sk_68c675733b6c83146700f1d5b798cb47ce718aa6f781161c");

        var response = await _ellevenLabsHttpClient.PostAsync(_ellevenLabsHttpClient.BaseAddress + "/FTNCalFNG5bRnkkaP5Ug/with-timestamps", content);
        if (response.IsSuccessStatusCode)
        {
            string res = await response.Content.ReadAsStringAsync();
            AudioData audioData = new()
            {
                AudioFile = "file/not/found 2"
            };
            return audioData;
        }
        else
        {
            // Handle the error
            AudioData audioData = new()
            {
                AudioFile = "file/not/found 2"
            };
            return audioData;
        }
    }
}