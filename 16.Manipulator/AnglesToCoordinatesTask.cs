using System;
using Avalonia;
using NUnit.Framework;
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
		var elbowPos = new Point(0, UpperArm);
		var wristPos = new Point(Forearm, UpperArm);
		var palmEndPos = new Point((Forearm + Palm), UpperArm);
		return new[]
		{
			elbowPos,
			wristPos,
			palmEndPos
		};
	}
}

[TestFixture]
public class AnglesToCoordinatesTask_Tests
{
	// Доработайте эти тесты!
	// С помощью строчки TestCase можно добавлять новые тестовые данные.
	// Аргументы TestCase превратятся в аргументы метода.
	[TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Forearm + Palm, UpperArm)]
	public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
	{
		var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
		Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
		Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
		Assert.Fail("TODO: проверить, что расстояния между суставами равны длинам сегментов манипулятора!");
	}
}