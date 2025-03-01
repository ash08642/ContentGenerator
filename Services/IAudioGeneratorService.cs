using ContentGenerator.Models;

namespace ContentGenerator.Services;

public interface IAudioGeneratorService
{
    public Task<AudioData?> GenerateAudio(string query);
}