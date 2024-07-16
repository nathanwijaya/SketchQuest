using SplashKitSDK;
namespace SketchQuest;

/// <summary>
/// Represents a text element in the UI. 
/// Extends the base UIElement class.
/// </summary>
public class Text : UIElement
{
    private string _text;
    private int _fontSize;
    private Color _color;
    private Font _font;
    private bool _animate;
    private float _animateScale;
    public string TextContent
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
        }
    }

    /// <summary>
    /// Initialises a new instance of the Text class.
    /// </summary>
    public Text(double x, double y, string text, int fontSize, Color color, Font font, bool animate) : base(x, y)
    {
        _text = text;
        _fontSize = fontSize;
        _color = color;
        _font = font;
        _animate = animate;

        if (animate)
        {
            _animateScale = 0.9f;
        }
    }

    /// <summary>
    /// Draws the text element on the screen, centered on the X and Y coordinates.
    /// </summary>
    public override void Draw()
    {
        double textWidth = SplashKit.TextWidth(_text, _font, _fontSize);
        double textHeight = SplashKit.TextHeight(_text, _font, _fontSize);

        double x = X - (textWidth / 2);
        double y = Y - (textHeight / 2);

        if (_animate)
        {
            // Adapted from: https://stackoverflow.com/questions/67322860/how-do-i-make-a-simple-idle-bobbing-motion-animation
            const float pulseSpeed = 0.005f;
            const float pulseAmount = 0.035f;
            float time = SplashKit.CurrentTicks();
            _animateScale = 0.9f + pulseAmount * MathF.Sin(time * pulseSpeed);

            Bitmap textBitmap = SplashKit.CreateBitmap("Text", 400, 100);
            SplashKit.DrawTextOnBitmap(textBitmap, _text, _color, _font, _fontSize, 0, 0);
            SplashKit.DrawBitmap(textBitmap, x, y, SplashKit.OptionScaleBmp(_animateScale, _animateScale));
        }
        else
        {
            SplashKit.DrawText(_text, _color, _font, _fontSize, x, y);
        }
    }
}
