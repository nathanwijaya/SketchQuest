using SplashKitSDK;
namespace SketchQuest;

/// <summary>
/// Defines a contract for clickable UI elements.
/// </summary>
public interface IClickable
{
    void OnClick();
    bool IsClicked(Point2D p);

    /// <summary>
    /// Event raised when the element is clicked.
    /// </summary>
    event EventHandler Clicked;

    public UIElementId Id
    {
        get;
    }

    public bool IsSelected
    {
        get; set;
    }
}
