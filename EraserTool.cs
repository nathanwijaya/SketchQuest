using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Represents an eraser tool that can remove pixels from a Drawing.
/// </summary>
public class EraserTool : IDrawingTool
{
    /// <summary>
    /// Initialises a new instance of the EraserTool class.
    /// </summary>
    public EraserTool()
    {
    }

    /// <summary>
    /// Overrides the Draw method to erase pixels at a specific location in the given Drawing.
    /// </summary>
    public void Draw(Drawing d, Point2D p, int radius)
    {
        d.RemovePixels(p.X, p.Y, radius);
    }
}
