using ContentGenerator.Models;

namespace ContentGenerator.Services;

public interface IContentService
{
    public Task CreateContent(Content content);
    public Task<Content?> UpdateContent(int id, Content content);
    public Task<Content?> GetContent(int id);
    public Task<List<Content>> GetAllContents();
    public Task DeleteContent(int id);
}