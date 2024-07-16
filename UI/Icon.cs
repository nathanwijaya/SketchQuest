using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Represents an interactive icon on the user interface.
/// </summary>
public class Icon : UIElement, IClickable
{
    private Bitmap _image;
    private UIElementId _id;
    private double _scale;
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

    /// <summary>
    /// Initialises a new instance of the Icon class with a default scale of 1.
    /// </summary>
    public Icon(double x, double y, Bitmap image, UIElementId id) : base(x, y)
    {
        _image = image;
        _id = id;
        _scale = 1;
        _isSelected = false;

    }

    /// <summary>
    /// Initialises a new instance of the Icon class with a specified scale.
    /// </summary>
    public Icon(double x, double y, Bitmap image, double scale, UIElementId id) : base(x, y)
    {
        _image = image;
        _id = id;
        _scale = scale;
        _isSelected = false;
    }

    /// <summary>
    /// Draws the icon on the screen.
    /// </summary>
    public override void Draw()
    {
        if (_isSelected)
        {
            DrawOutline();
        }
        SplashKit.DrawBitmap(_image, X, Y, SplashKit.OptionScaleBmp(_scale, _scale));
    }

    /// <summary>
    /// Raises the Clicked event.
    /// </summary>
    public void OnClick()
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Checks if the icon is clicked based on the mouse position.
    /// </summary>
    public bool IsClicked(Point2D p)
    {
        return X <= p.X && p.X <= X + SplashKit.BitmapWidth(_image) && Y <= p.Y && p.Y <= Y + SplashKit.BitmapHeight(_image);
    }

    /// <summary>
    /// Draws an outline around the icon.
    /// </summary>
    private void DrawOutline()
    {
        SplashKit.FillRectangle(Color.Aqua, X - 7.5, Y - 7.5, SplashKit.BitmapWidth(_image) * _scale + 8, SplashKit.BitmapHeight(_image) * _scale + 8);
    }
}
