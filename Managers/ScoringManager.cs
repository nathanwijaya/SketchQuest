using SplashKitSDK;

namespace SketchQuest;

/// <summary>
/// Manages scoring logic based on difference between reference image and drawing.
/// </summary>
public static class ScoringManager
{
    // RGB values of each pen color
    private static List<Color> PenColors = new List<Color> {
        Color.White, Color.Gray, Color.Red, Color.RGBAColor(255, 165, 0, 255), Color.Yellow, Color.Green, Color.Cyan, Color.RGBAColor(245, 121, 206, 255), Color.Black, Color.RGBAColor(61, 61, 61, 255), Color.RGBAColor(102, 0, 0, 255), Color.RGBAColor(150, 75, 0, 255), Color.RGBAColor(139, 128, 0, 255), Color.RGBAColor(4, 74, 0, 255), Color.Blue, Color.RGBAColor(133, 22, 201, 255)
    };

    /// <summary>
    /// Calculates the score based on the pixel differences between reference image and drawing.
    /// </summary>
    public static int CalculateScore(Bitmap image, Bitmap drawing)
    {
        int totalDifference = 0;
        int backgroundPoints = 0;
        Color[,] imagePixels = image.GetPixels();
        Color[,] drawingPixels = drawing.GetPixels();

        for (int row = 0; row < drawing.Height; row++)
        {
            for (int col = 0; col < drawing.Width; col++)
            {
                Color imagePixel = imagePixels[row, col];
                Color drawingPixel = drawingPixels[row, col];

                totalDifference += CalculatePixelDifference(imagePixel, drawingPixel, ref backgroundPoints);
            }
        }

        // Account for background pixels in the final score calculation
        int maxDifferencePossible = (100 * image.Width * image.Height) - backgroundPoints;
        int score = (int)Math.Round(100 - (totalDifference / (double)maxDifferencePossible * 100));

        return score;
    }

    /// <summary>
    /// Calculates the difference between a single pixel in the image and the drawing.
    /// </summary>
    private static int CalculatePixelDifference(Color imagePixel, Color drawingPixel, ref int backgroundPoints)
    {
        if (IsPixelTransparent(imagePixel))
        {
            return HandleBackgroundPixel(drawingPixel, ref backgroundPoints);
        }
        else if (IsPixelTransparent(drawingPixel))
        {
            // Penalise for not drawing where the image isn't transparent
            return 100;
        }
        else
        {
            // Evaluate colour difference for non-transparent pixels
            return CalculateColorDifference(imagePixel, drawingPixel);
        }
    }

    /// <summary>
    /// Returns whether or not the pixel is transparent (has an alpha value of 0).
    /// </summary>
    private static bool IsPixelTransparent(Color pixel)
    {
        return pixel.A == 0;
    }

    /// <summary>
    /// Handles scoring logic specific to transparent/background pixels in the image.
    /// </summary>
    private static int HandleBackgroundPixel(Color drawingPixel, ref int backgroundPoints)
    {
        // If player has drawn on the background (transparent pixel), they are penalised
        if (!IsPixelTransparent(drawingPixel))
            return 50;

        // If not, then exclude the background pixel from the final score
        backgroundPoints += 100;
        return 0;
    }

    /// <summary>
    /// Determines the color difference between the two pixels.
    /// </summary>
    private static int CalculateColorDifference(Color imagePixel, Color drawingPixel)
    {
        List<Color> imagePenRankings = FindClosestPenColors(imagePixel);
        Color drawingPenColor = FindClosestPenColors(drawingPixel).First();

        int ranking = 0;
        for (int i = 0; i < imagePenRankings.Count; i++)
        {
            if (imagePenRankings[i].Equals(drawingPenColor))
            {
                // If the drawing pen color is the same as the image pen color, set ranking to the index of the drawing pen color in the list
                ranking = i;
                break;
            }
        }

        // Calculate difference based on ranking (lower is better)
        return GetDifferenceFromRanking(ranking);
    }

    /// <summary>
    /// Fetches a predefined difference value based on the ranking.
    /// </summary>
    private static int GetDifferenceFromRanking(int ranking)
    {
        int[] differenceArray = { 0, 7, 9, 12, 18, 23, 27, 35, 45, 55, 67, 84, 85, 86, 89, 90 };
        return differenceArray[ranking];
    }

    /// <summary>
    /// Measures color difference using Euclidean distance
    /// </summary>
    // Adapted from https://stackoverflow.com/questions/9018016/how-to-compare-two-colors-for-similarity-difference and https://en.wikipedia.org/wiki/Color_difference
    private static double EuclideanDistance(Color color1, Color color2)
    {
        double redDifference = Math.Abs(color1.R - color2.R);
        double greenDifference = Math.Abs(color1.G - color2.G);
        double blueDifference = Math.Abs(color1.B - color2.B);
        return Math.Sqrt(redDifference * redDifference + greenDifference * greenDifference + blueDifference * blueDifference);
    }

    /// <summary>
    /// Ranks the pen colors in order of similarity to the pixel color.
    /// </summary>
    private static List<Color> FindClosestPenColors(Color pixelColor)
    {
        Dictionary<Color, double> colorDifferences = new Dictionary<Color, double>();

        for (int i = 0; i < PenColors.Count; i++)
        {
            double colorDifference = EuclideanDistance(PenColors[i], pixelColor);

            colorDifferences.Add(PenColors[i], colorDifference);
            if (colorDifference == 0)
            {
                // If the difference is 0, then the pixel color is the same as the pen color, so return it immediately
                break;
            }
        }

        // Order colors by difference and convert to list
        List<Color> orderedColorDifferences = colorDifferences.OrderBy(x => x.Value).Select(x => x.Key).ToList();

        return orderedColorDifferences;
    }



}
