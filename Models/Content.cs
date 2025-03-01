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
    public AudioData? AudioData {get; set;}
}

public class WordDuration
{
    public string Word { get; set; } = string.Empty;
    public double StartMs { get; set; }
    public double EndMs { get; set; }
}

public class AudioData
{
    public string AudioFile { get; set; } = string.Empty;
    public double AudioLengthInSeconds { get; set; }
    public List<WordDuration> WordDurations { get; set; } = [];
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