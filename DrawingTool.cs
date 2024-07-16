using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Defines a contract for drawing tools.
/// </summary>
public interface IDrawingTool
{
    // <summary>
    /// Performs the drawing operation using this tool on a given Drawing.
    /// </summary>
    public void Draw(Drawing d, Point2D p, int radius);
}
