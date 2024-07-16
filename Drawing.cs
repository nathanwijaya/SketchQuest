using System.Drawing;
using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Represents a drawing consisting of pixels. Provides methods to add and remove pixels, as well as to export the drawing.
/// </summary>
public class Drawing
{
    private List<Pixel> _pixels;
    private Bitmap _bitmap;

    /// <summary>
    /// Initialises a new Drawing object.
    /// </summary>
    public Drawing()
    {
        _pixels = new List<Pixel>();
        _bitmap = SplashKit.CreateBitmap("drawing", 500, 500);
    }

    /// <summary>
    /// Adds a pixel to the drawing.
    /// </summary>
    public void AddPixel(double x, double y, int radius, SplashKitSDK.Color color)
    {
        _pixels.Add(new Pixel(x, y, radius, color));
    }

    /// <summary>
    /// Removes pixels within a specified radius around the (x, y) coordinates.
    /// </summary>
    public void RemovePixels(double x, double y, int radius)
    {
        Circle eraser = new Circle
        {
            Center = new Point2D() { X = x, Y = y },
            Radius = radius
        };

        // Use LINQ to filter out pixels that intersect with the eraser circle
        _pixels = _pixels.Where(pixel =>
        {
            Circle pixelCircle = new Circle
            {
                Center = new Point2D() { X = pixel.X, Y = pixel.Y },
                Radius = pixel.Radius
            };
            // Check if the pixel circle intersects with the eraser circle
            return !SplashKit.CirclesIntersect(eraser, pixelCircle);
        }).ToList();
    }

    /// <summary>
    /// Exports the current drawing to a bitmap.
    /// </summary>
    public Bitmap Export()
    {
        // Draw pixels on bitmap instead of screen for export
        foreach (Pixel pixel in _pixels)
        {
            pixel.AddBitmapOffset();
            pixel.Draw(_bitmap);
        }
        return _bitmap;
    }

    /// <summary>
    /// Draws the current pixels on the screen.
    /// </summary>
    public void Draw()
    {
        foreach (Pixel pixel in _pixels)
        {
            // Draw pixels on screen instead of bitmap for better performance
            pixel.Draw();
        }
    }
}
