using System;
using System.Collections.Generic;

/// <summary>
/// Class containing the methods of Bresenham's line algorithm.
/// </summary>
public class Bresenham
{
    /// <summary>
    /// Calculates all the points in a line between the two supplied points using Bresenham's line algorithm.
    /// </summary>
    /// <param name="p0">The first point.</param>
    /// <param name="p1">The second point.</param>
    /// <returns>An IEnumerable that iterates over the points in the line.</returns>
    /// <example>
    /// <code>
    /// var point1 = (1, 2);
    /// var point2 = (4, 6);
    /// foreach(var point in Bresenham.New(point1, point2)){
    ///     Console.WriteLine(point);
    /// }
    /// </code>
    /// </example>
    public static IEnumerable<ValueTuple<int, int>> New(ValueTuple<int, int> p0, ValueTuple<int, int> p1)
    {
        return New(p0.Item1, p0.Item2, p1.Item1, p1.Item2); // Just unwrap the tuples and call the method that takes separate params.
    }

    /// <summary>
    /// Calculates all the points in a line between the two supplied points using Bresenham's line algorithm.
    /// </summary>
    /// <param name="x0">The x coordinate of the first point.</param>
    /// <param name="y0">The y coordinate of the first point.</param>
    /// <param name="x1">The x coordinate of the second point.</param>
    /// <param name="y1">The y coordinate of the second point.</param>
    /// <returns>an IEnumerable that iterates over the points in the line.</returns>
    /// <example>
    /// <code>
    /// foreach(var point in Bresenham.New((1,2), (4, 6))){
    ///     Console.WriteLine(point);
    /// }
    /// </code>
    /// </example>
    public static IEnumerable<ValueTuple<int, int>> New(int x0, int y0, int x1, int y1)
    {
        // Calculate differences (deltas) between the two points.
        int deltaX = Math.Abs(x1 - x0);
        int deltaY = -Math.Abs(y1 - y0);

        // Calculate slopes.
        int slopeX = x0 < x1 ? 1 : -1;
        int slopeY = y0 < y1 ? 1 : -1;

        // Variable for accumulating error.
        int error = deltaX + deltaY;

        // Loop where we calculate and return the points for the line.
        while (true)
        {
            yield return (x0, y0); // Return the point for the iteration to for example plot in a bitmap.
            if (x0 == x1 && y0 == y1) yield break; // Break if we are out of the loop

            int error_2 = error * 2; // Make a temporary variable for comparing.

            // If we have accumulated enough error increment x by its slope value.
            if (error_2 >= deltaY)
            {
                error += deltaY;
                x0 += slopeX;
            }

            // Likewise for y.
            if(error_2 <= deltaX)
            {
                error += deltaX;
                y0 += slopeY;
            }
        }
    }
}