using SplashKitSDK;
namespace SketchQuest;

/// <summary>
/// Represents the end screen state of the game.
/// </summary>
public class EndScreen : State
{
    private Button _saveToGalleryButton;

    private int _score;

    /// <summary>
    /// Initialises a new instance of the EndScreen class.
    /// </summary>
    public EndScreen(bool endGame, bool passed, bool isHighscore, int score)
    {
        _score = score;
        try
        {
            // Add text based on level completion status
            if (passed && !endGame)
            {
                _elements.Add(new Text(501.5, 100, "Level complete!", 60, Color.Black, _titleFont, false));
                _elements.Add(new Button(426.5, 275, 150, Color.RGBColor(50, 168, 82), UIElementId.NEXT_LEVEL, "Next Level"));
            }
            else if (endGame)
            {
                _elements.Add(new Text(501.5, 100, "You win!", 60, Color.Black, _titleFont, false));
            }
            else
            {
                _elements.Add(new Text(501.5, 100, "Game over!", 60, Color.Black, _titleFont, false));
            }

            _elements.Add(new Text(501.5, 200, "You scored " + score + " out of " + GameManager.GetInstance().ScoreRequirement + " points required!", 25, Color.Black, _textFont, false));

            if (isHighscore)
            {
                _elements.Add(new Text(501.5, 237.5, "New highscore!", 25, Color.Gold, _textFont, false));
            }

            _elements.Add(new Text(55, 20, "Level " + GameManager.GetInstance().LevelNumber + "/" + GameManager.GetInstance().LevelCount, 20, Color.Black, _textFont, false));

            _saveToGalleryButton = new Button(401.5, 350, 200, Color.Blue, UIElementId.SAVE_TO_GALLERY, "Save to Gallery");
            _elements.Add(_saveToGalleryButton);
            _elements.Add(new Button(426.5, 500, 150, Color.Red, UIElementId.MAIN_MENU, "Main Menu"));
        }
        catch (Exception)
        {
            throw new Exception("Error loading end screen fonts");
        }

        SubscribeToElementClicks();
    }

    /// <summary>
    /// Draws the elements on the end screen.
    /// </summary>
    public override void Draw()
    {
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
            case UIElementId.NEXT_LEVEL:
                GameManager.GetInstance().NextLevel();
                break;
            case UIElementId.MAIN_MENU:
                GameManager.GetInstance().ChangeState(new MainMenu());
                break;
            case UIElementId.SAVE_TO_GALLERY:
                _elements.Remove(_saveToGalleryButton);
                Gallery.AddDrawing(_score);
                _elements.Add(new Text(501.5, 375, "Saved to gallery!", 25, Color.Green, _textFont, false));
                break;
        }
    }

}
