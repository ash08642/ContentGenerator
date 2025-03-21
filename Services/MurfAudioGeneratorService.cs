using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using ContentGenerator.Models;

namespace ContentGenerator.Services;

public class MurfAudioGeneratorService : IAudioGeneratorService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GemeniTextGeneratorService> _logger;
    private readonly HttpClient _murfHttpClient;

    public MurfAudioGeneratorService(IHttpClientFactory httpClientFactory, ILogger<GemeniTextGeneratorService> logger)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _murfHttpClient = _httpClientFactory.CreateClient("Murf");
    }
    public async Task<AudioData> GenerateAudio(string query)
    {
        string rawResponse = await MurfGetReq(query);
        if (rawResponse == "")
        {
            return new AudioData {
                AudioFile = "file/not/found"
            };
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        GemeniAudioData? gemeniAudioData = JsonSerializer.Deserialize<GemeniAudioData>(rawResponse, options);
        AudioData audioData = new()
        {
                AudioFile = "file/not/found 2"
        };
        if (gemeniAudioData != null)
        {
            audioData.AudioFile = gemeniAudioData.AudioFile;
            _logger.LogInformation("Logging geminiAudioData");
            _logger.LogInformation(gemeniAudioData.ToString());
            audioData.AudioLengthInSeconds = gemeniAudioData.AudioLengthInSeconds;
            for (int i = 0; i < gemeniAudioData.WordDurations.Count; i++)
            {
                audioData.WordDurations.Add(new WordDuration{
                    StartMs = gemeniAudioData.WordDurations[i].StartMs,
                    EndMs = gemeniAudioData.WordDurations[i].EndMs,
                    Word = gemeniAudioData.WordDurations[i].Word
                });
            };
        }
        return audioData;
    }

    public async Task<string> MurfGetReq(string text)
    {
        var body = new
        {
            voiceId = "de-DE-matthias",
            style = "Conversational",
            text = text,
            rate = 0,
            pitch = 0,
            sampleRate = 48000,
            format = "MP3",
            channelType = "MONO",
            pronunciationDictionary = new { },
            encodeAsBase64 = false,
            variation = 1,
            audioDuration = 0,
            modelVersion = "GEN2",
            multiNativeLocale = "de-DE"
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