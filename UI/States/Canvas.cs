using SplashKitSDK;
namespace SketchQuest;

/// <summary>
/// Represents the canvas UI for the levels.
/// </summary>
public class Canvas : State
{
    private IDrawingTool _currentDrawingTool;
    private int _pixelRadius;
    private bool _gridEnabled;
    private Drawing _drawing;
    private Bitmap _referenceImage;
    private IClickable _selectedTool;
    private IClickable _selectedColor;
    private IClickable _selectedPenSize;
    private PenTool _penTool;

    private Bitmap _canvasBackground;

    private Dictionary<UIElementId, Color> _penColors;

    public Bitmap Drawing
    {
        get
        {
            return _drawing.Export();
        }
    }

    /// <summary>
    /// Initialises a new instance of the Canvas class.
    /// </summary>
    public Canvas(Bitmap referenceImage, int highscore)
    {
        _referenceImage = referenceImage;
        _drawing = new Drawing();
        _pixelRadius = 6;
        _gridEnabled = true;
        _penTool = new PenTool();
        _currentDrawingTool = _penTool;

        try
        {
            _canvasBackground = AssetManager.GetInstance().GetBitmap("canvas-background");
        }
        catch (Exception)
        {
            throw new Exception("Error loading gallery background");
        }

        // UI
        _penColors = new Dictionary<UIElementId, Color>()
        {
             { UIElementId.WHITE_COLOR, Color.White },
             { UIElementId.GREY_COLOR, Color.Gray },
             { UIElementId.RED_COLOR, Color.Red },
             { UIElementId.ORANGE_COLOR, Color.RGBAColor(255, 165, 0, 255) },
             { UIElementId.YELLOW_COLOR, Color.Yellow },
             { UIElementId.GREEN_COLOR, Color.Green },
             { UIElementId.CYAN_COLOR, Color.Cyan },
             { UIElementId.PINK_COLOR, Color.RGBAColor(245, 121, 206, 255) },
             { UIElementId.BLACK_COLOR, Color.Black },
             { UIElementId.DARK_GREY_COLOR, Color.RGBAColor(61, 61, 61, 255) },
             { UIElementId.MAROON_COLOR, Color.RGBAColor(102, 0, 0, 255) },
             { UIElementId.BROWN_COLOR, Color.RGBAColor(150, 75, 0, 255) },
             { UIElementId.GOLD_COLOR, Color.RGBAColor(139, 128, 0, 255) },
             { UIElementId.DARK_GREEN_COLOR, Color.RGBAColor(4, 74, 0, 255) },
             { UIElementId.BLUE_COLOR, Color.Blue },
             { UIElementId.PURPLE_COLOR, Color.RGBAColor(133, 22, 201, 255) }
        };

        AddPenColors();
        AddUIElements(highscore);

        SubscribeToElementClicks();
    }

    /// <summary>
    /// Adds the UI elements to the canvas.
    /// </summary>
    private void AddUIElements(int highscore)
    {
        try
        {
            Bitmap timerIcon = AssetManager.GetInstance().GetBitmap("timer-icon");
            Bitmap penIcon = AssetManager.GetInstance().GetBitmap("pencil-icon");
            Bitmap eraserIcon = AssetManager.GetInstance().GetBitmap("eraser-icon");
            Bitmap gridIcon = AssetManager.GetInstance().GetBitmap("grid-icon");

            Icon penElement = new Icon(860, 520, penIcon, 1.4, UIElementId.PEN_TOOL);
            _selectedTool = (IClickable?)penElement;
            penElement.IsSelected = true;

            Icon gridElement = new Icon(947, 520, gridIcon, 1.4, UIElementId.GRID_TOOL);
            if (_gridEnabled)
            {
                gridElement.IsSelected = true;
            }

            _elements.Add(new Icon(217, 528, timerIcon, 1.2, UIElementId.NOT_CLICKABLE));
            _elements.Add(penElement);
            _elements.Add(new Icon(902, 520, eraserIcon, 1.4, UIElementId.ERASER_TOOL));
            _elements.Add(gridElement);
        }
        catch (Exception)
        {
            throw new Exception("Error loading UI assets");
        }

        _elements.Add(new Text(238, 570, "Highscore: " + highscore, 20, Color.Black, _textFont, false));
        _elements.Add(new Text(55, 20, "Level " + GameManager.GetInstance().LevelNumber + "/" + GameManager.GetInstance().LevelCount, 20, Color.Black, _textFont, false));

        CircleButton medPenSize = new CircleButton(882, 570, 9, Color.Black, UIElementId.MED_PEN_SIZE);
        _selectedPenSize = medPenSize;
        medPenSize.IsSelected = true;

        _elements.Add(new CircleButton(860, 570, 5, Color.Black, UIElementId.SMALL_PEN_SIZE));
        _elements.Add(medPenSize);
        _elements.Add(new CircleButton(912, 570, 13, Color.Black, UIElementId.LARGE_PEN_SIZE));
        _elements.Add(new CircleButton(950, 570, 16, Color.Black, UIElementId.XL_PEN_SIZE));
    }

    /// <summary>
    /// Adds the pen color options to the canvas.
    /// </summary>
    private void AddPenColors()
    {
        int index = 1;

        foreach (KeyValuePair<UIElementId, Color> penColor in _penColors)
        {
            double x;
            double y;

            if (index < 9)
            {
                x = 483.5 + (40 * index);
                y = 530;
            }
            else
            {
                x = 483.5 + (40 * (index - 8));
                y = 570;
            }

            CircleButton colorButton = new CircleButton(x, y, 15, penColor.Value, penColor.Key);

            _elements.Add(colorButton);

            // Select black as the default color
            if (penColor.Key == UIElementId.BLACK_COLOR)
            {
                colorButton.IsSelected = true;
                _selectedColor = colorButton;
            }

            index++;
        }
    }

    /// <summary>
    /// Draws the canvas UI elements on the screen.
    /// </summary>
    public override void Draw()
    {
        try
        {
            SplashKit.DrawBitmap(_canvasBackground, 0, 0);
            // Draw UI background
            SplashKit.FillRectangle(Color.RGBColor(219, 219, 219), 0, 500, 1003, 100);
            // Draw canvas separator line
            SplashKit.FillRectangle(Color.Black, 500, 0, 3, 500);

            // Draw timer
            Color timerTextColor = Color.Black;
            if (Timer.GetInstance().SecondsRemaining < 11)
            {
                timerTextColor = Color.Red;
            }

            Text timerText = new Text(255, 535, Timer.GetInstance().SecondsRemaining.ToString(), 25, timerTextColor, _textFont, false);
            timerText.Draw();

            DrawElements();
            _drawing.Draw();

            SplashKit.DrawBitmap(_referenceImage, 0, 0);
            if (_gridEnabled)
            {
                DrawGrid();
            }
        }
        catch (Exception)
        {
            throw new Exception("Error loading font");
        }
    }

    /// <summary>
    /// Handles mouse-down events on the canvas for drawing.
    /// </summary>
    public void MouseDown(Point2D p)
    {
        if (p.X > 503 && p.Y < 500)
        {
            _currentDrawingTool.Draw(_drawing, p, _pixelRadius);
        }

    }

    /// <summary>
    /// Handles the click event for the UI elements.
    /// </summary>
    protected override void HandleElementClick(object sender, EventArgs e)
    {
        IClickable button = (IClickable)sender;

        foreach (KeyValuePair<UIElementId, Color> penColor in _penColors)
        {
            if (button.Id == penColor.Key)
            {
                _penTool.Color = penColor.Value;

                if (_selectedColor != null)
                {
                    // Deselect the previously selected color
                    _selectedColor.IsSelected = false;
                }

                // Select the new color
                button.IsSelected = true;
                _selectedColor = button;
                return;
            }
        }

        switch (button.Id)
        {
            case UIElementId.PEN_TOOL:
                _currentDrawingTool = _penTool;
                SelectTool(button);
                break;
            case UIElementId.ERASER_TOOL:
                _currentDrawingTool = new EraserTool();
                SelectTool(button);
                break;
            case UIElementId.GRID_TOOL:
                _gridEnabled = !_gridEnabled;
                button.IsSelected = _gridEnabled;
                break;
            case UIElementId.SMALL_PEN_SIZE:
                SelectPenSize(3, button);
                break;
            case UIElementId.MED_PEN_SIZE:
                SelectPenSize(6, button);
                break;
            case UIElementId.LARGE_PEN_SIZE:
                SelectPenSize(10, button);
                break;
            case UIElementId.XL_PEN_SIZE:
                SelectPenSize(12, button);
                break;
        }
    }

    /// <summary>
    /// Selects the pen size for drawing.
    /// </summary>
    private void SelectPenSize(int penSize, IClickable button)
    {
        _pixelRadius = penSize;
        if (_selectedPenSize != null)
        {
            _selectedPenSize.IsSelected = false;
        }

        button.IsSelected = true;
        _selectedPenSize = button;
    }

    /// <summary>
    /// Selects the tool to use for drawing.
    /// </summary>
    private void SelectTool(IClickable button)
    {
        if (_selectedTool != null)
        {
            _selectedTool.IsSelected = false;
        }

        button.IsSelected = true;
        _selectedTool = button;
    }

    /// <summary>
    /// Draws the grid on the canvas.
    /// </summary>
    private void DrawGrid()
    {
        int index = 0;
        int lines = 3;
        int lineGap = 500 / (lines + 1);
        Color lineColor = Color.RGBAColor(0, 0, 0, 40);

        while (index < lines)
        {
            // Draw grid on reference image
            SplashKit.DrawLine(lineColor, 0, lineGap * (index + 1), 500, lineGap * (index + 1));
            SplashKit.DrawLine(lineColor, lineGap * (index + 1), 0, lineGap * (index + 1), 500);

            // Draw grid on canvas drawing
            SplashKit.DrawLine(lineColor, 503, lineGap * (index + 1), 1003, lineGap * (index + 1));
            SplashKit.DrawLine(lineColor, lineGap * (index + 1) + 503, 0, lineGap * (index + 1) + 503, 500);
            index++;
        }
    }
}
