using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Manages countdown timer for each level.
/// </summary>
public class Timer
{
    private static Timer _instance;
    private int _secondsRemaining;

    public int SecondsRemaining
    {
        get
        {
            return _secondsRemaining;
        }
    }

    /// <summary>
    /// Private constructor to enforce Singleton design pattern.
    /// Initialises the timer.
    /// </summary>
    private Timer()
    {
        _secondsRemaining = -1;
    }

    /// <summary>
    /// Provides access to the singleton Timer instance.
    /// </summary>
    public static Timer GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Timer();
        }
        return _instance;
    }

    /// <summary>
    /// Starts the timer with the given number of seconds.
    /// </summary>
    public void Start(int seconds)
    {
        _secondsRemaining = seconds;
    }

    /// <summary>
    /// Decrements the timer and checks if time has run out.
    /// </summary>
    public void Tick()
    {
        _secondsRemaining--;

        if (_secondsRemaining == 10)
        {
            SplashKit.PlaySoundEffect(AssetManager.GetInstance().GetSoundEffect("clock-ticking"));
            SplashKit.FadeMusicOut(15000);
        }

        if (_secondsRemaining == 0)
        {
            SplashKit.StopSoundEffect(AssetManager.GetInstance().GetSoundEffect("clock-ticking"));
            SplashKit.PlaySoundEffect(AssetManager.GetInstance().GetSoundEffect("times-up"));
            _secondsRemaining = -1;
            GameManager.GetInstance().EndLevel();
        }
    }


}
