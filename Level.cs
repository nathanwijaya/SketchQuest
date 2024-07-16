namespace SketchQuest;
using SplashKitSDK;

/// <summary>
/// Represents level in the game with associated properties like reference image and highscore.
/// </summary>
public class Level
{
    private Bitmap _referenceImage;
    private int _highscore;
    private string _name;
    public Bitmap ReferenceImage
    {
        get
        {
            return _referenceImage;
        }
    }

    public int Highscore
    {
        get
        {
            return _highscore;
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }
    }

    /// <summary>
    /// Initialises a new level with a reference image loaded from the given image name.
    /// </summary>
    public Level(string imageName)
    {
        _name = imageName;
        _referenceImage = SplashKit.LoadBitmap(imageName, Path.Combine(AssetManager.GetInstance().GetFilePath("level-images") + imageName + ".png")); ;
    }
}


