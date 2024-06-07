using Avalonia;
using NUnit.Framework;
using System;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class AnglesToCoordinatesTask
{
    /// <summary>
    /// По значению углов суставов возвращает массив координат суставов
    /// в порядке new []{elbow, wrist, palmEnd}
    /// </summary>
    public static Point[] GetJointPositions(double shoulder, double elbow, double wrist)
    {
        var elbowPos = CalculatePosition(shoulder, UpperArm, 0, 0);
        var cumulativeAngle = shoulder + elbow - Math.PI;
        var wristPos = CalculatePosition(cumulativeAngle, Forearm, elbowPos.X, elbowPos.Y);
        cumulativeAngle = cumulativeAngle + wrist - Math.PI;
        var palmEndPos = CalculatePosition(cumulativeAngle, Palm, wristPos.X, wristPos.Y);

        return new[]
        {
            elbowPos,
            wristPos,
            palmEndPos
        };
    }

    private static Point CalculatePosition(double angle, float length, double x, double y)
    {
        var cosAngle = Math.Cos(angle);
        var sinAngle = Math.Sin(angle);
        var positionX = cosAngle * length;
        var positionY = sinAngle * length;
        return new Point(positionX + x, positionY + y);
    }
}

[TestFixture]
public class AnglesToCoordinatesTask_Tests
{
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Forearm + Palm, UpperArm)]
    [TestCase(Math.PI / 2, Math.PI / 2, Math.PI / 2, Forearm, UpperArm - Palm)]
    [TestCase(Math.PI / 2, 3 * Math.PI / 2, 3 * Math.PI / 2, -Forearm, UpperArm - Palm)]
    [TestCase(Math.PI / 2, Math.PI, 3 * Math.PI, 0, Forearm + UpperArm + Palm)]

    public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
    {
        var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
        Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
        Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        Assert.AreEqual(GetDistance(joints[0], new Point(0, 0)), UpperArm, 1e-5);
        Assert.AreEqual(GetDistance(joints[0], joints[1]), Forearm, 1e-5);
        Assert.AreEqual(GetDistance(joints[1], joints[2]), Palm, 1e-5);
    }

    public double GetDistance(Point point1, Point point2)
    {
        var differenceX = (point1.X - point2.X) * (point1.X - point2.X);
        var differenceY = (point1.Y - point2.Y) * (point1.Y - point2.Y);
        return Math.Sqrt(differenceX + differenceY);
    }
}
