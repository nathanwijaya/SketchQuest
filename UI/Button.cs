using SplashKitSDK;
namespace SketchQuest;


/// <summary>
/// Represents a clickable button UI element.
/// </summary>
public class Button : UIElement, IClickable
{
    private Color _color;
    private UIElementId _id;
    private string _text;
    private int _width;

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

    public Button(double x, double y, int width, Color color, UIElementId id, string text) : base(x, y)
    {
        _color = color;
        _id = id;
        _text = text;
        _width = width;
        _isSelected = false;
    }

    /// <summary>
    /// Draws the button element on the screen.
    /// </summary>
    public override void Draw()
    {
        try
        {
            // Positions button text in the middle of the button
            Text buttonText = new Text(X + (_width / 2), Y + 25, _text, 20, GetReadableTextColor(_color), AssetManager.GetInstance().GetFont("Roboto"), false);
            SplashKit.FillRectangle(_color, X, Y, _width, 50);
            buttonText.Draw();
        }
        catch (Exception)
        {
            throw new Exception("Error loading button font");
        }
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
        return X <= p.X && p.X <= X + _width && Y <= p.Y && p.Y <= Y + 50;
    }

    /// <summary>
    /// Determines a readable text color based on the background color of the button.
    /// </summary>
    /// <returns>A color that is readable against the button's background color.</returns>
    private Color GetReadableTextColor(Color buttonColor)
    {
        // Calculate the luminance of the button color (source: https://www.w3.org/TR/AERT/#color-contrast)
        double luminance = 299 * buttonColor.R + 587 * buttonColor.G + 114 * buttonColor.B;

        // Determine the text color based on the luminance
        if (luminance > 600)
        {
            return Color.Black;
        }
        else
        {
            return Color.White;
        }
    }
}
