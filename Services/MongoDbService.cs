using ContentGenerator.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ContentGenerator.Services;

public class MongoDbService : IContentDbService, IUserDbService
{
    private readonly MongoClient _client;
    private readonly IMongoCollection<Content> _contentCollection;
    private readonly IMongoCollection<BsonDocument> _userCollection;
    public MongoDbService()
    {
        var connectionString = "mongodb://localhost:27017/";
        _client = new MongoClient(connectionString);
        _contentCollection = _client.GetDatabase("ContentGenerator").GetCollection<Content>("Contents");
        _userCollection = _client.GetDatabase("ContentGenerator").GetCollection<BsonDocument>("Users");
    }

    public async Task<IEnumerable<Content>> Content_GetAsync()
    {
        return await _contentCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Content?> Content_GetAsync(string id)
    {
        return await _contentCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task Content_CreateAsync(Content content)
    {
        await _contentCollection.InsertOneAsync(content);
    }

    public async Task<bool> Content_UpdateAsync(string id, Content updatedContent)
    {
        var res = await _contentCollection.ReplaceOneAsync(x => x.Id == id, updatedContent);
        if (!res.IsAcknowledged)
        {
            return false;
        }
        else
        {
            if (res.ModifiedCount < 1)
            {
                return false;
            }
        }
        return true;
    }

    public async Task<bool> Content_RemoveAsync(string id)
    {
        var res = await _contentCollection.DeleteOneAsync(x => x.Id == id);
        if (!res.IsAcknowledged)
        {
            return false;
        }
        else
        {
            if (res.DeletedCount < 1)
            {
                return false;
            }
        }
        return true;
    }
}