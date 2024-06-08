using System;
using NUnit.Framework;
using static Manipulation.Manipulator;

namespace Manipulation;

public static class ManipulatorTask
{
	/// <summary>
	/// Возвращает массив углов (shoulder, elbow, wrist),
	/// необходимых для приведения эффектора манипулятора в точку x и y 
	/// с углом между последним суставом и горизонталью, равному alpha (в радианах)
	/// </summary>
	public static double[] MoveManipulatorTo(double x, double y, double alpha)
	{
		var wristX = x + Palm * Math.Cos(Math.PI - alpha);
		var wristY = y + Palm * Math.Sin(Math.PI - alpha);
        var wristVectorLength = Math.Sqrt(wristX * wristX + wristY * wristY);

        var elbowAngle = TriangleTask.GetABAngle(UpperArm, Forearm, wristVectorLength);
		var shoulderAngle = (TriangleTask.GetABAngle(UpperArm, wristVectorLength, Forearm)) + (Math.Atan2(wristY, wristX));
		var wristAngle = -alpha - shoulderAngle - elbowAngle;
        if (double.IsNaN(wristAngle) ||  double.IsNaN(elbowAngle) || double.IsNaN(shoulderAngle))
		    return new[] { double.NaN, double.NaN, double.NaN };
        return new[] { shoulderAngle, elbowAngle, wristAngle };
	}
}

[TestFixture]
public class ManipulatorTask_Tests
{
	[Test]
    public void TestMoveManipulatorTo()
    {
        var random = new Random();
        for (int i = 0; i < 1000; i++)
        {
            var x = random.NextDouble() * 20 - 10; 
            var y = random.NextDouble() * 20 - 10;
            var alpha = random.NextDouble() * 2 * Math.PI;

            var angles = ManipulatorTask.MoveManipulatorTo(x, y, alpha);

            if (Math.Sqrt(x * x + y * y) > UpperArm + Forearm + Palm)
            {
                Assert.IsTrue(double.IsNaN(angles[0]));
                Assert.IsTrue(double.IsNaN(angles[1]));
                Assert.IsTrue(double.IsNaN(angles[2]));
            }
            else
            {
                var effectorPos = AnglesToCoordinatesTask.GetJointPositions(angles[0], angles[1], angles[2])[2];
                Assert.AreEqual(x, effectorPos.X, 1e-6);
                Assert.AreEqual(y, effectorPos.Y, 1e-6);
            }
        }
    }
}