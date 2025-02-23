using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContentGenerator.Models;

public class Content
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get; set;}
    public string Text {get; set;} = string.Empty;
    public ContentType Type {get; set;}
    public Tongue Tongue {get; set;}
    public string AudioPath {get; set;} = string.Empty;
}

public enum ContentType
{
    Story,
    Presentation,
    Conversational
}

public enum Tongue
{
    Englisch,
    Deutsch,
    French
}