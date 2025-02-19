using ContentGenerator.Models;

namespace ContentGenerator.Services;

public interface ITextGeneratorService
{
    public Task<string?> GenerateText(string query);
}