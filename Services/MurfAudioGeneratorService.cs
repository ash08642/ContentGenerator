using System.Text.Json;
using System.Text.Json.Nodes;
using ContentGenerator.Models;

namespace ContentGenerator.Services;

public class MurfAudioGeneratorService : IAudioGeneratorService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ILogger<GemeniTextGeneratorService> _logger;
    public MurfAudioGeneratorService(IHttpClientService httpClientService, ILogger<GemeniTextGeneratorService> logger)
    {
        _httpClientService = httpClientService;
        _logger = logger;
    }
    public async Task<AudioData?> GenerateAudio(string query)
    {
        string rawResponse = await _httpClientService.MurfGetReq(query);
        _logger.LogInformation("Logging GenerateAudio");
        _logger.LogInformation(rawResponse);
        if (rawResponse == "")
        {
            return new AudioData {
                AudioFile = "file/not/found"
            };
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        GemeniAudioData? gemeniAudioData = JsonSerializer.Deserialize<GemeniAudioData>(rawResponse, options);
        AudioData audioData = new AudioData {
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
}