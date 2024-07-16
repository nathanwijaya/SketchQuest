namespace SketchQuest;
using SplashKitSDK;

/// <summary>
/// Represents a pixel that can be drawn on a Drawing.
/// </summary>
public class Pixel
{
    private double _x;
    private double _y;
    private int _radius;
    private Color _color;

    public double X
    {
        get
        {
            return _x;
        }
    }

    public double Y
    {
        get
        {
            return _y;
        }
    }

    public int Radius
    {
        get
        {
            return _radius;
        }
    }

    /// <summary>
    /// Initialises a new instance of the Pixel class.
    /// </summary>
    public Pixel(double x, double y, int radius, Color color)
    {
        _x = x;
        _y = y;
        _radius = radius;
        _color = color;
    }

    /// <summary>
    /// Draws the pixel on the screen.
    /// </summary>
    public void Draw()
    {
        SplashKit.FillCircle(_color, _x, _y, _radius);
    }

    /// <summary>
    /// Draws the pixel on a given bitmap.
    /// </summary>
    public void Draw(Bitmap bitmap)
    {
        SplashKit.FillCircleOnBitmap(bitmap, _color, _x, _y, _radius);
    }

    /// <summary>
    /// Adjusts the pixel's X coordinate to account for offset when drawing on a bitmap.
    /// </summary>
    public void AddBitmapOffset()
    {
        // 503 is the offset between the bitmap and the screen
        _x -= 503;
    }
}
