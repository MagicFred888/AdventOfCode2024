using System.Drawing;

namespace AdventOfCode2024.Extensions;

public static class PointExtensions
{
    public static long ManhattanDistanve(this Point point, Point target)
    {
        return Math.Abs(point.X - target.X) + Math.Abs(point.Y - target.Y);
    }

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

    public static Point Rotate180Degree(this Point point, Point centerPoint)
    {
        int x = point.X - centerPoint.X;
        int y = point.Y - centerPoint.Y;
        return new Point(centerPoint.X - x, centerPoint.Y - y);
    }

    public static Point Subtract(this Point p1, Point p2)
    {
        return new Point(p1.X - p2.X, p1.Y - p2.Y);
    }

    public static Point Add(this Point p1, Point p2)
    {
        return new Point(p1.X + p2.X, p1.Y + p2.Y);
    }

    public static Point Add(this Point p1, int x, int y)
    {
        return new Point(p1.X + x, p1.Y + y);
    }

    public static Point Multiply(this Point p, int factor)
    {
        return new Point(p.X * factor, p.Y * factor);
    }
}