using System.Drawing;
using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Represents an abstract UI element that can be drawn on the screen.
/// </summary>
public abstract class UIElement
{
    private double _x;
    private double _y;
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

    /// <summary>
    /// Initialises a new instance of the UIElement class with given X and Y coordinates.
    /// </summary>
    public UIElement(double x, double y)
    {
        _x = x;
        _y = y;
    }

    /// <summary>
    /// Abstract method to draw the UI element.
    /// </summary>
    public abstract void Draw();
}
