using SplashKitSDK;
namespace SketchQuest;

/// <summary>
/// Represents the main menu of the game, allowing players to start new games or view the gallery.
/// </summary>
public class MainMenu : State
{
    private Bitmap _background;

    /// <summary>
    /// Initialises a new instance of the MainMenu class.
    /// </summary>
    public MainMenu()
    {
        try
        {
            _background = AssetManager.GetInstance().GetBitmap("main-menu-background");

            _elements.Add(new Text(501.5, 100, "SketchQuest", 70, Color.White, _titleFont, true));
            _elements.Add(new Text(501.5, 220, "Play", 30, Color.White, _textFont, false));
            _elements.Add(new Button(266.5, 260, 150, Color.RGBColor(73, 184, 103), UIElementId.START_GAME_EASY, "Easy"));
            _elements.Add(new Button(426.5, 260, 150, Color.RGBColor(50, 168, 82), UIElementId.START_GAME_MEDIUM, "Medium"));
            _elements.Add(new Button(586.5, 260, 150, Color.RGBColor(32, 145, 62), UIElementId.START_GAME_HARD, "Hard"));
            _elements.Add(new Button(351.5, 400, 300, Color.Blue, UIElementId.VIEW_GALLERY, "View Gallery"));
        }
       catch (Exception ex)
{
    Console.WriteLine($"Error loading main menu assets: {ex.Message}");
    throw; // Rethrows the current exception
}

        SubscribeToElementClicks();
    }

    /// <summary>
    /// Draws the main menu on the screen.
    /// </summary>
    public override void Draw()
    {
        SplashKit.DrawBitmap(_background, 0, 0);
        SplashKit.FillRectangle(Color.RGBAColor(0, 0, 0, 0.8), 220, 0, 560, 600);
        DrawElements();
    }

    /// <summary>
    /// Handles the click event for the UI elements.
    /// </summary>
    protected override void HandleElementClick(object sender, EventArgs e)
    {
        IClickable button = (IClickable)sender;
        switch (button.Id)
        {
            case UIElementId.START_GAME_EASY:
                GameManager.GetInstance().NewGame(Difficulty.EASY);
                break;
            case UIElementId.START_GAME_MEDIUM:
                GameManager.GetInstance().NewGame(Difficulty.MEDIUM);
                break;
            case UIElementId.START_GAME_HARD:
                GameManager.GetInstance().NewGame(Difficulty.HARD);
                break;
            case UIElementId.VIEW_GALLERY:
                GameManager.GetInstance().ChangeState(Gallery.GetInstance());
                break;
        }
    }
}
