namespace SketchQuest;

/// <summary>
/// Represents an entry in the asset library, containing information about an asset.
/// </summary>
public class AssetEntry
{
    public Dictionary<string, string> Images
    {
        get;
        set;
    }
    public Dictionary<string, string> Fonts
    {
        get;
        set;
    }
        public Dictionary<string, string> Music
    {
        get;
        set;
    }
        public Dictionary<string, string> SoundEffects
    {
        get;
        set;
    }
    public Dictionary<string, string> Files
    {
        get;
        set;
    }
}
