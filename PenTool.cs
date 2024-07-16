using System.Drawing;
using SplashKitSDK;
using Color = SplashKitSDK.Color;

namespace SketchQuest;

/// <summary>
/// Represents a pen tool that can be used for drawing.
/// </summary>
public class PenTool : IDrawingTool
{
    private Color _color;
    public Color Color
    {
        get
        {
            return _color;
        }
        set
        {
            _color = value;
        }
    }

    /// <summary>
    /// Initialises a new instance of the PenTool class with default color as black.
    /// </summary>
    public PenTool()
    {
        _color = Color.Black;
    }

    /// <summary>
    /// Overrides the Draw method to add a pixel at a given point and radius on the given Drawing.
    /// </summary>
    public void Draw(Drawing d, Point2D p, int radius)
    {
        d.AddPixel(p.X, p.Y, radius, _color);
    }
}
