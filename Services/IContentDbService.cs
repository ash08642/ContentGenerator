using ContentGenerator.Models;

namespace ContentGenerator.Services;

public interface IContentDbService
{
    public Task<IEnumerable<Content>> Content_GetAsync();
    public Task<Content?> Content_GetAsync(string id);

    public Task Content_CreateAsync(Content content);

    public Task<bool> Content_UpdateAsync(string id, Content updatedContent);

    public Task<bool> Content_RemoveAsync(string id);
}