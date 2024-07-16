using System.Text.Json;
namespace SketchQuest;

/// <summary>
/// Manages highscore data including loading, updating, and saving highscores.
/// </summary>
public static class HighscoreManager
{
    /// <summary>
    /// Updates or creates a highscore entry based on the provided score, difficulty, and level.
    /// </summary>
    public static void UpdateHighscore(int score, Difficulty difficulty, Level level)
    {
        List<HighscoreEntry> highscores = LoadHighscores();

        // Check if there is already a highscore for this level and difficulty
        HighscoreEntry existingHighscore = FetchHighscoreEntry(difficulty, level);

        // If there is an existing highscore, check if the new score is better
        if (existingHighscore != null)
        {
            if (score > existingHighscore.Score)
            {
                // If the new score is better, update the highscore
                int index = highscores.FindIndex(h => h.LevelName == level.Name && h.Difficulty == difficulty.ToString());

                highscores[index].Score = score;
            }
        }
        else
        {
            // If there is no existing highscore, create a new one
            HighscoreEntry newHighscore = new HighscoreEntry
            {
                LevelName = level.Name,
                Difficulty = difficulty.ToString(),
                Score = score
            };
            highscores.Add(newHighscore);
        }

        // Serialise the highscore list to JSON and save to file
        try
        {
            string json = JsonSerializer.Serialize(highscores);
            File.WriteAllText(AssetManager.GetInstance().GetFilePath("highscore-data"), json);
        }
        catch (Exception)
        {
            throw new Exception("Error saving highscores");
        }
    }

    /// <summary>
    /// Fetches the highscore for a given difficulty and level.
    /// </summary>
    public static int FetchHighscore(Difficulty difficulty, Level level)
    {
        // Check if there is already a highscore for this level and difficulty
        HighscoreEntry existingHighscore = FetchHighscoreEntry(difficulty, level);

        // If there is an existing highscore, return it
        if (existingHighscore != null)
        {
            return existingHighscore.Score;
        }
        else
        {
            // If there is no existing highscore, return 0
            return 0;
        }
    }

    /// <summary>
    /// Fetches the HighscoreEntry object for a given difficulty and level.
    /// </summary>
    private static HighscoreEntry FetchHighscoreEntry(Difficulty difficulty, Level level)
    {
        List<HighscoreEntry> highscores = LoadHighscores();

        // Check if there is already a highscore for this level and difficulty
        HighscoreEntry existingHighscore = highscores.Find(h => h.LevelName == level.Name && h.Difficulty == difficulty.ToString());

        return existingHighscore;
    }

    /// <summary>
    /// Loads the highscore list from a JSON file, or creates a new list if the file doesn't exist.
    /// </summary>
    private static List<HighscoreEntry> LoadHighscores()
    {
        try
        {
            // Deserialize the JSON data from the file and load it into the highscores list
            string json = File.ReadAllText(AssetManager.GetInstance().GetFilePath("highscore-data"));
            List<HighscoreEntry> highscores = JsonSerializer.Deserialize<List<HighscoreEntry>>(json);
            return highscores;
        }
        catch (FileNotFoundException)
        {
            // If the file doesn't exist, create a new list of highscores
            List<HighscoreEntry> highscores = new List<HighscoreEntry>();
            string json = JsonSerializer.Serialize(highscores);
            File.WriteAllText(AssetManager.GetInstance().GetFilePath("highscore-data"), json);
            return highscores;
        }
    }
}
