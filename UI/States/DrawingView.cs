using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Represents the state for displaying a drawing with some metadata.
/// </summary>
public class DrawingView : State
{
    private Bitmap _image;
    private Bitmap _drawing;
    private Bitmap _background;
    private GalleryEntry _drawingData;

    /// <summary>
    /// Initialises a new DrawingView instance with the specified GalleryEntry data.
    /// </summary>
    public DrawingView(GalleryEntry drawingData)
    {
        try
        {
            _drawingData = drawingData;
            _drawing = SplashKit.LoadBitmap(_drawingData.ImageName, Path.Combine(AssetManager.GetInstance().GetFilePath("gallery-images") + _drawingData.ImageName));
            _image = GameManager.GetInstance().FetchLevel(_drawingData.LevelNumber).ReferenceImage;
            _background = AssetManager.GetInstance().GetBitmap("canvas-background");

            // UI elements initialisation
            _elements.Add(new Button(800, 525, 150, Color.Red, UIElementId.BACK_TO_GALLERY, "Back"));
            _elements.Add(new Text(251.5, 530, "Score: " + _drawingData.Score, 25, Color.Black, _textFont, false));
            _elements.Add(new Text(251.5, 570, _drawingData.Date.ToLocalTime().ToString(), 25, Color.Black, _textFont, false));

        }
        catch (Exception)
        {
            throw new Exception("Error loading DrawingView assets");
        }

        SubscribeToElementClicks();
    }

    /// <summary>
    /// Draws the view on the screen.
    /// </summary>
    public override void Draw()
    {
        SplashKit.DrawBitmap(_background, 0, 0);
        // Draw separator line
        SplashKit.FillRectangle(Color.Black, 500, 0, 3, 500);
        SplashKit.DrawBitmap(_image, 0, 0);
        SplashKit.DrawBitmap(_drawing, 503, 0);
        DrawElements();
    }

    /// <summary>
    /// Handles click events for UI elements.
    /// </summary>
    protected override void HandleElementClick(object sender, EventArgs e)
    {
        IClickable button = (IClickable)sender;
        switch (button.Id)
        {
            case UIElementId.BACK_TO_GALLERY:
                GameManager.GetInstance().ChangeState(Gallery.GetInstance());
                break;
        }
    }

}
