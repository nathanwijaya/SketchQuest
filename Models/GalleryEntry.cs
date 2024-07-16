namespace SketchQuest;

/// <summary>
/// Represents an entry in the gallery, containing information about a drawing.
/// </summary>
public class GalleryEntry
{
    public int LevelNumber
    {
        get;
        set;
    }

    public string Difficulty
    {
        get;
        set;
    }

    public int Score
    {
        get;
        set;
    }

    public string ImageName
    {
        get;
        set;
    }

    public DateTime Date
    {
        get;
        set;
    }
}
