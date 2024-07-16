using SplashKitSDK;
using SkiaSharp;

namespace SketchQuest;

/// <summary>
/// Provides extension methods for bitmap manipulations and conversions.
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// Gets the pixels in a Bitmap object and converts them to a multidimensional array of Color objects.
    /// </summary>
    public static Color[,] GetPixels(this Bitmap bitmap)
    {
        SKBitmap skBitmap = ConvertBitmapToSKBitmap(bitmap);

        int width = skBitmap.Width;
        int height = skBitmap.Height;

        // Create a multidimensional array to store RGBA values
        Color[,] pixels = new Color[width, height];

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                SKColor color = skBitmap.GetPixel(row, col);

                Color splashKitColor = Color.RGBAColor(color.Red, color.Green, color.Blue, color.Alpha);

                // Extract RGBA values and store them in the array
                pixels[row, col] = splashKitColor;
            }
        }
        return pixels;
    }

    /// <summary>
    /// Converts a SplashKit Bitmap to a SkiaSharp SKBitmap.
    /// </summary>
    private static SKBitmap ConvertBitmapToSKBitmap(Bitmap bitmap)
    {
        string imageName = "image";
        try
        {
            SplashKit.SaveBitmap(bitmap, imageName);
            MoveImageFromDesktop(imageName + ".png");
            return SKBitmap.Decode(Path.Combine(AssetManager.GetInstance().GetFilePath("temp-data") + imageName + ".png"));
        }
        catch (Exception)
        {
            throw new Exception("Error converting bitmap to SKBitmap");
        }
    }

    /// <summary>
    /// Moves an image file from the desktop to a specified directory.
    /// </summary>
    private static void MoveImageFromDesktop(string imageName)
    {
        try
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string sourceFilePath = Path.Combine(desktopPath, imageName);
            string destinationFilePath = Path.Combine(AssetManager.GetInstance().GetFilePath("temp-data") + imageName);
            File.Move(sourceFilePath, destinationFilePath, true);
        }
        catch (Exception)
        {
            throw new Exception("Error moving image from desktop");
        }
    }
}
