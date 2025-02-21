using ContentGenerator.Models;

namespace ContentGenerator.Services;

public interface IContentService
{
    public Task<bool> CreateContent(Content content);
    public Task<bool> UpdateContent(Guid id, Content content);
    public Task<Content?> GetContent(Guid id);
    public Task<IEnumerable<Content>> GetAllContents();
    public Task<bool> DeleteContent(Guid id);
}