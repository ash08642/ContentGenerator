namespace ContentGenerator.Models;

public class GemeniAudioData
{
    public string AudioFile { get; set; }
    public string EncodedAudio { get; set; }
    public double AudioLengthInSeconds { get; set; }
    public List<GemeniWordDuration> WordDurations { get; set; }
    public string Warning { get; set; }
    public int ConsumedCharacterCount { get; set; }
    public int RemainingCharacterCount { get; set; }
}

public class GemeniWordDuration
{
    public string Word { get; set; }
    public int StartMs { get; set; }
    public int EndMs { get; set; }
    public int SourceWordIndex { get; set; }
    public double PitchScaleMinimum { get; set; }
    public double PitchScaleMaximum { get; set; }
}