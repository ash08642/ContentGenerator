namespace ContentGenerator.Models;

public class Content
{
    public Guid Id {get; set;}
    public string Text {get; set;} = string.Empty;
    public ContentType Type {get; set;}
    public string AudioPath {get; set;} = string.Empty;
}

public enum ContentType
{
    Story,
    Presentation,
    Conversational
}