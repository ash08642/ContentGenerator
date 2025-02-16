using ContentGenerator.Models;

namespace ContentGenerator.Services;

public class ContentService : IContentService
{
    private static readonly List<Content> AllContents = [];

    public Task CreateContent(Content content) {
        AllContents.Add(content);
        return Task.CompletedTask;
    }

    public Task<Content?> UpdateContent(int id, Content content)
    {
        Content content1 = AllContents.FirstOrDefault(x => x.Id == id);
        if (content1 != null)
        {
            content1.Text = content.Text;
            content1.AudioPath = content.AudioPath;
        }
        return Task.FromResult(content1);
    }

    public Task<Content?> GetContent(int id) 
    {
        return Task.FromResult(AllContents.FirstOrDefault(x => x.Id == id));
    }

    public Task<List<Content>> GetAllContents()
    {
        return Task.FromResult(AllContents);
    }

    public Task DeleteContent(int id)
    {
        Content content = AllContents.FirstOrDefault(x => x.Id == id);
        if (content != null)
        {
            AllContents.Remove(content);
        }
        return Task.CompletedTask;
    }
}