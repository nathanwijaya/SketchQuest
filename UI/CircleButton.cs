using SplashKitSDK;
namespace SketchQuest;

/// <summary>
/// Represents a circle button UI element.
/// Extends the base UIElement class.
/// </summary>
public class CircleButton : UIElement, IClickable
{
    private Color _color;
    private UIElementId _id;
    private int _radius;
    private bool _isSelected;

    public event EventHandler Clicked;

    public UIElementId Id
    {
        get
        {
            return _id;
        }
    }

    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
        }
    }

    public CircleButton(double x, double y, int radius, Color color, UIElementId id) : base(x, y)
    {
        _id = id;
        _radius = radius;
        _color = color;
        _isSelected = false;
    }

    /// <summary>
    /// Draws the circle button on the screen.
    /// </summary>
    public override void Draw()
    {
        if (_isSelected)
        {
            DrawOutline();
        }
        SplashKit.FillCircle(_color, base.X, base.Y, _radius);

    }

    /// <summary>
    /// Raises the Clicked event.
    /// </summary>
    public void OnClick()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Checks if the button is clicked based on the mouse position.
    /// </summary>
    public bool IsClicked(Point2D p)
    {
        Circle c = new Circle();
        Point2D pt = new Point2D();
        pt.X = base.X;
        pt.Y = base.Y;
        c.Center = pt;
        c.Radius = _radius;
        return SplashKit.PointInCircle(p, c);
    }

    /// <summary>
    /// Draws an outline around the circle button.
    /// </summary>
    private void DrawOutline()
    {
        Color outlineColor = SplashKit.RGBColor(255, 255, 255);
        SplashKit.FillCircle(outlineColor, base.X, base.Y, _radius + 2);
    }

}
