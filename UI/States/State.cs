using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Represents a game state, manages the UI elements and click events within each state.
/// </summary>
public abstract class State
{
    protected List<UIElement> _elements;
    protected Font _titleFont;
    protected Font _textFont;

    /// <summary>
    /// Initialises a new instance of the State class.
    /// </summary>
    public State()
    {
        _elements = new List<UIElement>();
        // Font source: https://github.com/zedseven/Pixellari
        _titleFont = AssetManager.GetInstance().GetFont("Pixellari");
        // Font source: https://fonts.google.com/specimen/Roboto
        _textFont = AssetManager.GetInstance().GetFont("Roboto");
    }

    protected void SubscribeToElementClicks()
    {
        // Subscribe to the Clicked event of each button
        foreach (IClickable element in _elements.OfType<IClickable>())
        {
            element.Clicked += HandleElementClick;
        }
    }

    /// <summary>
    /// Handles click events within the state, delegating the click to the clicked UI element.
    /// </summary>
    public virtual void Click(Point2D p)
    {
        foreach (IClickable element in _elements.OfType<IClickable>().ToList())
        {
            if (element.IsClicked(p))
            {
                element.OnClick();
            }
        }
    }

    /// <summary>
    /// Abstract method for drawing the state.
    /// </summary>
    public abstract void Draw();

    /// <summary>
    /// Draws all UI elements associated with this state.
    /// </summary>
    protected void DrawElements()
    {
        foreach (UIElement element in _elements)
        {
            element.Draw();
        }
    }

    /// <summary>
    /// Abstract method for handling element clicks within the state.
    /// </summary>
    protected abstract void HandleElementClick(object sender, EventArgs e);
}


