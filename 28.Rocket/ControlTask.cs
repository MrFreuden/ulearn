using System;

namespace func_rocket;

public class ControlTask
{
    private static double _totalAngle;
    public static Turn ControlRocket(Rocket rocket, Vector target)
    {
        Vector distanceVector = target - rocket.Location;

        if (Math.Abs(distanceVector.Angle - rocket.Direction) < 0.5
                || Math.Abs(distanceVector.Angle - rocket.Velocity.Angle) < 0.5)
        {
            _totalAngle = (distanceVector.Angle - rocket.Direction + distanceVector.Angle - rocket.Velocity.Angle) / 2;
        }
        else
        {
            _totalAngle = distanceVector.Angle - rocket.Direction;
        }

        if (_totalAngle < 0)
            return Turn.Left;
        return _totalAngle > 0 ? Turn.Right : Turn.None;
    }
}