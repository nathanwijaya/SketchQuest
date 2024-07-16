namespace SketchQuest;

/// <summary>
/// Represents an entry in the saved highscores data, containing the level name, difficulty and score achieved.
/// </summary>
public class HighscoreEntry
{
    public string LevelName
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
}
