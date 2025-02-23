using ContentGenerator.Models;

namespace ContentGenerator.Services;

public interface IContentService
{
    public Task CreateContent(Content content);
    public Task<bool> UpdateContent(string id, Content content);
    public Task<Content?> GetContent(string id);
    public Task<IEnumerable<Content>> GetAllContents();
    public Task<bool> DeleteContent(string id);
}