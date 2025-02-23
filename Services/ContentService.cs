using ContentGenerator.Data;
using ContentGenerator.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ContentGenerator.Services;

public class ContentService : IContentService
{
    private readonly IContentDbService _contentDbService;
    public ContentService(IContentDbService contentDbService)
    {
        _contentDbService = contentDbService;
    }

    public async Task CreateContent(Content content) {
        await _contentDbService.Content_CreateAsync(content);
    }

    public async Task<bool> UpdateContent(string id, Content content)
    {
        return await _contentDbService.Content_UpdateAsync(id, content);
    }

    public async Task<Content?> GetContent(string id) 
    {
        return await _contentDbService.Content_GetAsync(id);
    }

    public async Task<IEnumerable<Content>> GetAllContents()
    {
        return await _contentDbService.Content_GetAsync();
    }

    public async Task<bool> DeleteContent(string id)
    {
        return await _contentDbService.Content_RemoveAsync(id);
    }
}