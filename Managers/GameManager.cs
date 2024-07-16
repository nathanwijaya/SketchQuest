using System.Data;
using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Manages the game state.
/// </summary>
public class GameManager
{
    // Singleton instance of GameManager
    private static GameManager _instance;
    private State _state;

    private List<Level> _levels;
    private Difficulty _currentDifficulty;
    private int _currentLevel;

    public int ScoreRequirement
    {
        get
        {
            return GetScoreRequirement(_currentDifficulty);
        }
    }

    public int LevelNumber
    {
        get
        {
            return _currentLevel + 1;
        }
    }

    public int LevelCount
    {
        get
        {
            return _levels.Count;
        }
    }

    public Difficulty Difficulty
    {
        get
        {
            return _currentDifficulty;
        }
    }

    public int Score 
    {
        get
        {
            return ScoringManager.CalculateScore(GetCurrentLevel().ReferenceImage, ((Canvas)_state).Drawing);
        }
    }

    /// <summary>
    /// Private constructor to enforce Singleton design pattern.
    /// Initialises the game using the MainMenu state and predefined levels.
    /// </summary>
    private GameManager()
    {
        _state = new MainMenu();
        _levels = new List<Level>
        {
            new Level("apple"),
            new Level("pencil"),
            new Level("snowman")
        };
    }

    /// <summary>
    /// Provides access to the singleton GameManager instance.
    /// </summary>
    public static GameManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameManager();
        }
        return _instance;
    }

    /// <summary>
    /// Draws the current game state.
    /// </summary>
    public void Draw()
    {
        _state.Draw();
    }

    /// <summary>
    /// Handles a click event.
    /// </summary>
    public void Click(Point2D p)
    {
        _state.Click(p);
    }

    /// <summary>
    /// Starts a new game with the given difficulty setting.
    /// </summary>
    public void NewGame(Difficulty difficulty)
    {
        SplashKit.FadeMusicOut(500);
        SplashKit.PlaySoundEffect(AssetManager.GetInstance().GetSoundEffect("game-start"));
        SplashKit.FadeMusicIn(AssetManager.GetInstance().GetMusic("game"), 1000);
        _currentDifficulty = difficulty;
        _currentLevel = 0;
        _state = new Canvas(GetCurrentLevel().ReferenceImage, HighscoreManager.FetchHighscore(_currentDifficulty, GetCurrentLevel()));
        StartTimer();
    }

    /// <summary>
    /// Handles a mouse down event.
    /// </summary>
    public void MouseDown(Point2D p)
    {
        if (_state is Canvas canvasState)
        {
            canvasState.MouseDown(p);
        }
    }

    /// <summary>
    /// Ends the current level, updates highscore and displays end screen.
    /// </summary>
    public void EndLevel()
    {
        SplashKit.StopMusic();
        SplashKit.FadeMusicIn(AssetManager.GetInstance().GetMusic("main-menu"), 99, 1000);
        int score = CalculateScore();
        bool isHighscore = CheckIfHighscore(score);

        if (isHighscore)
        {
            HighscoreManager.UpdateHighscore(score, _currentDifficulty, GetCurrentLevel());
        }

        if (_currentLevel == _levels.Count - 1)
        {
            EndGame(score, isHighscore);
        }
        else
        {
            _state = new EndScreen(false, CheckIfPassed(score), isHighscore, score);
        }
    }

    /// <summary>
    /// Starts the next level and resets the timer.
    /// </summary>
    public void NextLevel()
    {
        SplashKit.FadeMusicOut(500);
        SplashKit.FadeMusicIn(AssetManager.GetInstance().GetMusic("game"), 1000);
        _currentLevel++;
        _state = new Canvas(GetCurrentLevel().ReferenceImage, HighscoreManager.FetchHighscore(_currentDifficulty, GetCurrentLevel()));
        StartTimer();
    }

    /// <summary>
    /// Displays the end screen.
    /// </summary>
    public void EndGame(int score, bool isHighscore)
    {
        ChangeState(new EndScreen(true, CheckIfPassed(score), isHighscore, score));
    }

    /// <summary>
    /// Changes the current game state.
    /// </summary>
    public void ChangeState(State state)
    {
        _state = state;
    }

    /// <summary>
    /// Starts the timer with duration based on current difficulty.
    /// </summary>
    private void StartTimer()
    {
        Timer.GetInstance().Start(GetTimerDuration(_currentDifficulty));
    }

    /// <summary>
    /// Checks if the player passed the level based on the current score and difficulty.
    /// </summary>
    private bool CheckIfPassed(int score)
    {
        return score >= GetScoreRequirement(_currentDifficulty);
    }

    /// <summary>
    /// Retrieves the current score based on the player's drawing.
    /// </summary>
    private int CalculateScore()
    {
        return ScoringManager.CalculateScore(GetCurrentLevel().ReferenceImage, ((Canvas)_state).Drawing);
    }

    /// <summary>
    /// Checks if the current score is a highscore for the current level and difficulty.
    /// </summary>
    private bool CheckIfHighscore(int score)
    {
        return score > HighscoreManager.FetchHighscore(_currentDifficulty, GetCurrentLevel());
    }

    /// <summary>
    /// Retrieves the score requirement for the given difficulty.
    /// </summary>
    private int GetScoreRequirement(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                return 50;
            case Difficulty.MEDIUM:
                return 60;
            case Difficulty.HARD:
                return 75;
            default:
                return 0;
        }
    }

    /// <summary>
    /// Retrieves the timer duration for the given difficulty.
    /// </summary>
    private int GetTimerDuration(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.EASY:
                return 60;
            case Difficulty.MEDIUM:
                return 50;
            case Difficulty.HARD:
                return 40;
            default:
                return 0;
        }
    }

    /// <summary>
    // Retrieves the Level object for the current level
    // </summary>
    private Level GetCurrentLevel()
    {
        return _levels[_currentLevel];
    }

    /// <summary>
    /// Retrieves the Level object for the given level number.
    /// </summary>
    public Level FetchLevel(int levelNumber)
    {
        return _levels[levelNumber - 1];
    }


}
