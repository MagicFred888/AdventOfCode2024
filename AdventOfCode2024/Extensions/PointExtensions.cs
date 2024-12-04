using System.Drawing;

namespace AdventOfCode2024.Extensions;

public static class PointExtensions
{
    public static Point RotateClockwise(this Point point, Point centerPoint)
    {
        int x = point.X - centerPoint.X;
        int y = point.Y - centerPoint.Y;
        return new Point(centerPoint.X + y, centerPoint.Y - x);
    }

    public static Point RotateCounterclockwise(this Point point, Point centerPoint)
    {
        int x = point.X - centerPoint.X;
        int y = point.Y - centerPoint.Y;
        return new Point(centerPoint.X - y, centerPoint.Y + x);
    }
}