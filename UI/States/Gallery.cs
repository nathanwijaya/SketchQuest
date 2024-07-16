using SplashKitSDK;
using System.Globalization;
using System.Text.Json;

namespace SketchQuest;

/// <summary>
/// Manages the gallery state.
/// </summary>
public class Gallery : State
{
    private static Gallery _instance;
    private static List<GalleryEntry> _drawings;
    private List<List<GalleryEntry>> _displayedDrawings;
    private int _currentPage;
    private Difficulty _difficultyFilter;
    private Bitmap _background;

    private Text _difficultyText;
    private Text _pageNavText;

    /// <summary>
    /// Private constructor to enforce Singleton design pattern.
    /// Initialises a new instance of the Gallery class.
    /// </summary>
    private Gallery()
    {
        _drawings = LoadDrawings();
        _currentPage = 0;
        _difficultyFilter = Difficulty.EASY;
        _displayedDrawings = new List<List<GalleryEntry>>();
        UpdateDisplayedDrawings(_difficultyFilter);

        try
        {
            _background = AssetManager.GetInstance().GetBitmap("gallery-background");
        }
        catch (Exception)
        {
            throw new Exception("Error loading gallery background");
        }

        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

        _difficultyText = new Text(490, 110, textInfo.ToTitleCase(_difficultyFilter.ToString().ToLower()) + " mode", 30, Color.White, _textFont, false);
        _pageNavText = new Text(490, 435, "--", 30, Color.White, _textFont, false);
        UpdatePageNavText();

        _elements.Add(new Text(490, 50, "Gallery", 60, Color.White, _titleFont, false));
        _elements.Add(_difficultyText);
        _elements.Add(_pageNavText);
        _elements.Add(new Button(363.5, 410, 50, Color.Yellow, UIElementId.PREV_GALLERY_PAGE, "<"));
        _elements.Add(new Button(563.5, 410, 50, Color.Yellow, UIElementId.NEXT_GALLERY_PAGE, ">"));
        _elements.Add(new Button(415, 500, 150, Color.Red, UIElementId.MAIN_MENU, "Back"));

        _elements.Add(new Button(30, 410, 150, Color.RGBColor(73, 184, 103), UIElementId.EASY_MODE_GALLERY, "Easy"));
        _elements.Add(new Button(30, 470, 150, Color.RGBColor(50, 168, 82), UIElementId.MEDIUM_MODE_GALLERY, "Medium"));
        _elements.Add(new Button(30, 530, 150, Color.RGBColor(32, 145, 62), UIElementId.HARD_MODE_GALLERY, "Hard"));

        SubscribeToElementClicks();
    }

    /// <summary>
    /// Provides access to the singleton Gallery instance.
    /// </summary>
    public static Gallery GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Gallery();
        }
        return _instance;
    }

    /// <summary>
    /// Adds a new drawing to the gallery.
    /// </summary>
    public static void AddDrawing(int score)
    {
        SaveDrawingFile();
        try
        {
            List<GalleryEntry> galleryData = LoadDrawings();
            GalleryEntry newDrawing = new GalleryEntry
            {
                LevelNumber = GameManager.GetInstance().LevelNumber,
                Difficulty = GameManager.GetInstance().Difficulty.ToString(),
                Score = score,
                ImageName = "drawing_" + DateTime.Now.ToString("yyyy-mm-dd-hh-mm-ss") + ".png",
                Date = DateTime.Now
            };
            galleryData.Add(newDrawing);
            string json = JsonSerializer.Serialize(galleryData);
            File.WriteAllText(AssetManager.GetInstance().GetFilePath("gallery-data"), json);
        }
        catch (Exception)
        {
            throw new Exception("Error saving drawing to gallery");
        }
    }

    /// <summary>
    /// Handles clicks within the gallery state.
    /// </summary>
    public override void Click(Point2D p)
    {
        // Handle UI element clicks
        foreach (IClickable element in _elements.OfType<IClickable>())
        {
            if (element.IsClicked(p))
            {
                element.OnClick();
            }
        }

        // Check if a drawing in the gallery has been clicked
        for (int i = 0; i < _displayedDrawings[_currentPage].Count; i++)
        {
            GalleryEntry drawing = _displayedDrawings[_currentPage][i];
            Bitmap image = SplashKit.LoadBitmap(drawing.ImageName, Path.Combine(AssetManager.GetInstance().GetFilePath("gallery-images") + drawing.ImageName));
            if (p.X >= 40 + (i * 230) && p.X <= 240 + (i * 230) && p.Y >= 170 && p.Y <= 370)
            {
                GameManager.GetInstance().ChangeState(new DrawingView(drawing));
            }
        }
    }

    /// <summary>
    /// Loads drawing entries from a JSON file.
    /// </summary>
    private static List<GalleryEntry> LoadDrawings()
    {
        try
        {
            // Deserialize the JSON data from the file and load it into the gallery data list
            string json = File.ReadAllText(AssetManager.GetInstance().GetFilePath("gallery-data"));
            List<GalleryEntry> galleryData = JsonSerializer.Deserialize<List<GalleryEntry>>(json);
            return galleryData;
        }
        catch (FileNotFoundException)
        {
            // If the file doesn't exist, create a new list of gallery data
            List<GalleryEntry> galleryData = new List<GalleryEntry>();
            string json = JsonSerializer.Serialize(galleryData);
            File.WriteAllText(AssetManager.GetInstance().GetFilePath("gallery-data"), json);
            return galleryData;
        }
    }

    /// <summary>
    /// Updates the drawings displayed based on the difficulty filter.
    /// </summary>
    private void UpdateDisplayedDrawings(Difficulty difficulty)
    {
        _drawings = LoadDrawings();
        _displayedDrawings.Clear();
        var filteredDrawings = _drawings.Where(d => d.Difficulty == difficulty.ToString()).ToList();
    
        // Add an empty page if there are no drawings to display
        if (!filteredDrawings.Any())
        {
            _displayedDrawings.Add(new List<GalleryEntry>()); 
            return;
        }
    
        int drawingsPerPage = 4;
        int pageCount = (int)Math.Ceiling((double)filteredDrawings.Count / drawingsPerPage);
    
        for (int i = 0; i < pageCount; i++)
        {
            _displayedDrawings.Add(new List<GalleryEntry>());
            for (int j = 0; j < drawingsPerPage; j++)
            {
                int drawingIndex = i * drawingsPerPage + j;
                if (drawingIndex < filteredDrawings.Count)
                {
                    _displayedDrawings[i].Add(filteredDrawings[drawingIndex]);
                }
            }
        }
    }

    /// <summary>
    /// Draws the gallery on the screen.
    /// </summary>
    public override void Draw()
    {
        SplashKit.DrawBitmap(_background, 0, 0);
        DrawElements();
        DrawDrawings();
    }

    /// <summary>
    /// Draws the gallery drawings on the screen.
    /// </summary>
    private void DrawDrawings()
    {
        List<GalleryEntry> toDraw = _displayedDrawings[_currentPage];

        for (int i = 0; i < toDraw.Count; i++)
        {
            try
            {
                GalleryEntry drawing = toDraw[i];
                Bitmap image = SplashKit.LoadBitmap(drawing.ImageName, Path.Combine(AssetManager.GetInstance().GetFilePath("gallery-images") + drawing.ImageName));
                SplashKit.DrawBitmap(image, -110 + (i * 230), 20, SplashKit.OptionScaleBmp(0.4, 0.4));
            }
            catch (Exception)
            {
                throw new Exception("Error loading gallery drawing");
            }
        }
    }

    /// <summary>
    /// Handles click events for UI elements.
    /// </summary>
    protected override void HandleElementClick(object sender, EventArgs e)
    {
        IClickable button = (IClickable)sender;
        switch (button.Id)
        {
            case UIElementId.MAIN_MENU:
                GameManager.GetInstance().ChangeState(new MainMenu());
                break;
            case UIElementId.NEXT_GALLERY_PAGE:
                if (_currentPage < _displayedDrawings.Count - 1)
                {
                    _currentPage++;
                }
                else
                {
                    _currentPage = 0;
                }
                UpdatePageNavText();
                break;
            case UIElementId.PREV_GALLERY_PAGE:
                if (_currentPage > 0)
                {
                    _currentPage--;
                }
                else
                {
                    _currentPage = _displayedDrawings.Count - 1;
                }
                UpdatePageNavText();
                break;
            case UIElementId.EASY_MODE_GALLERY:
                ChangeDifficultyFilter(Difficulty.EASY);
                break;
            case UIElementId.MEDIUM_MODE_GALLERY:
                ChangeDifficultyFilter(Difficulty.MEDIUM);
                break;
            case UIElementId.HARD_MODE_GALLERY:
                ChangeDifficultyFilter(Difficulty.HARD);
                break;
        }
    }

    /// <summary>
    /// Changes the difficulty filter and updates the display.
    /// </summary>
    private void ChangeDifficultyFilter(Difficulty difficulty)
    {
        _currentPage = 0;
        _difficultyFilter = difficulty;
        _difficultyText.TextContent = new CultureInfo("en-US", false).TextInfo.ToTitleCase(_difficultyFilter.ToString().ToLower()) + " mode";
        UpdateDisplayedDrawings(_difficultyFilter);
        UpdatePageNavText();
    }

    /// <summary>
    /// Saves the current drawing to the gallery folder.
    /// </summary>
    private static void SaveDrawingFile()
    {
        try
        {
            string sourceFilePath = Path.Combine(AssetManager.GetInstance().GetFilePath("temp-data") + "image.png");
            string destinationFilePath = Path.Combine(AssetManager.GetInstance().GetFilePath("gallery-images") + "drawing_" + DateTime.Now.ToString("yyyy-mm-dd-hh-mm-ss") + ".png");
            File.Move(sourceFilePath, destinationFilePath);
        }
        catch (System.Exception)
        {
            throw new System.Exception("Error saving drawing file to gallery");
        }
    }

    private void UpdatePageNavText()
    {
        _pageNavText.TextContent = _currentPage + 1 + "/" + _displayedDrawings.Count;
    }

}
