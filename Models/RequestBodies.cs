namespace ContentGenerator.Models;

public class TextRequest {
        public string Text { get; set; } = string.Empty;
        public Tongue Tongue { get; set; }
        public ContentType Type {get; set;}

}