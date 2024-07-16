using System.Text.Json;
using SplashKitSDK;
namespace SketchQuest;

/// <summary>
/// Manages asset data and provides methods for fetching assets.
/// </summary>
public class AssetManager
{
    private static AssetManager _instance;
    private static List<AssetEntry> _assetData;
    private static string _assetsPath;

    /// <summary>
    /// Private constructor to enforce Singleton design pattern.
    /// Initialises the asset manager.
    /// </summary>
    private AssetManager()
    {
        _assetsPath = "./assets/";
        _assetData = LoadAssetData();
    }

    /// <summary>
    /// Provides access to the singleton AssetManager instance.
    /// </summary>
    public static AssetManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new AssetManager();
        }
        return _instance;
    }

    /// <summary>
    /// Returns the Bitmap with the given ID.
    /// </summary>
    public Bitmap? GetBitmap(string assetId)
    {
        try
        {
            // Get the path to the bitmap from the asset data
            string bitmapPath = _assetData.Find(asset => asset.Images.ContainsKey(assetId)).Images[assetId];
            // Load the bitmap from the path
            return SplashKit.LoadBitmap(assetId, Path.Combine(_assetsPath, bitmapPath));
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the Font with the given ID.
    /// </summary>
    public Font? GetFont(string assetId)
    {
        try
        {
            // Get the path to the font from the asset data
            string fontPath = _assetData.Find(asset => asset.Fonts.ContainsKey(assetId)).Fonts[assetId];
            // Load the font from the path
            return SplashKit.LoadFont(assetId, Path.Combine(_assetsPath, fontPath));
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the Music with the given ID.
    /// </summary>
    public Music? GetMusic(string assetId)
    {
        try
        {
            // Get the path to the music from the asset data
            string musicPath = _assetData.Find(asset => asset.Music.ContainsKey(assetId)).Music[assetId];
            // Load the music from the path
            return SplashKit.LoadMusic(assetId, Path.Combine(_assetsPath, musicPath));
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the SoundEffect with the given ID.
    /// </summary>
    public SoundEffect? GetSoundEffect(string assetId)
    {
        try
        {
            // Get the path to the sound effect from the asset data
            string soundEffectPath = _assetData.Find(asset => asset.SoundEffects.ContainsKey(assetId)).SoundEffects[assetId];
            // Load the sound effect from the path
            return SplashKit.LoadSoundEffect(assetId, Path.Combine(_assetsPath, soundEffectPath));
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the filepath for the given ID.
    /// </summary>
    public string GetFilePath(string id)
    {
        try
        {
            // Get the path to the file from the asset data
            string filePath = _assetData.Find(asset => asset.Files.ContainsKey(id)).Files[id];
            return Path.Combine(_assetsPath, filePath);
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Loads the asset data from a JSON file.
    /// </summary>
    private static List<AssetEntry> LoadAssetData()
    {
        try
        {
            // Deserialize the JSON data from the file and load it into the assets list
            string json = File.ReadAllText(_assetsPath + "asset-data.json");
            List<AssetEntry> assets = JsonSerializer.Deserialize<List<AssetEntry>>(json);
            return assets;
        }
        catch (Exception)
        {
            throw new Exception("Error loading asset data");
        }
    }
}
