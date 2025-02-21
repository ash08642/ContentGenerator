using ContentGenerator.Data;
using ContentGenerator.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ContentGenerator.Services;

public class ContentService : IContentService
{
    private readonly ContentDbContext _context;
    public ContentService(ContentDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateContent(Content content) {
        if (_context.Contents == null)
        {
            return false;
        }
        _context.Contents.Add(content);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateContent(Guid id, Content content)
    {
         if (_context.Contents == null)
        {
            return false;
        }

        _context.Entry(content).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ContentExists(id))
            {
                return false;
            }
            else 
            {
                return false;
            }
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<Content?> GetContent(Guid id) 
    {
        var content = await _context.Contents.FindAsync(id);
        return content;
    }

    public async Task<IEnumerable<Content>> GetAllContents()
    {
        return await _context.Contents.ToListAsync();
    }

    public async Task<bool> DeleteContent(Guid id)
    {
        if (_context.Contents == null)
        {
            return false;
        }
        var content = await _context.Contents.FindAsync(id);
        if (content == null)
        {
            return false;
        }

        _context.Contents.Remove(content);
        await _context.SaveChangesAsync();
        return true;
    }

    private bool ContentExists(Guid id)
    {
        return (_context.Contents?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}