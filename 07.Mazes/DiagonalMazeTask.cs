using System;

namespace Mazes;

public static class DiagonalMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        var stepsX = width - 2 - robot.X;
        var stepsY = height - 2 - robot.Y;
        var minSide = Math.Min(stepsX, stepsY);
        for (int i = 0; i < minSide + 1; i++)
        {
            if (width > height)
                MoveRobotInTwoDirections(robot, (width - 2) / (height - 2), Direction.Right, Direction.Down);
            
            else
                MoveRobotInTwoDirections(robot, (height - 2) / (width - 2), Direction.Down, Direction.Right);
        }
    }

    private static void MoveRobotInTwoDirections(Robot robot, int stepMax, Direction direction, Direction direction2)
    {
        MoveTo(robot, stepMax, direction);
        if (robot.Finished)
            return;
        MoveTo(robot, 1, direction2);
    }

    private static void MoveTo(Robot robot, int stepCount, Direction direction)
    {
        for (int i = 0; i < stepCount; i++)
            robot.MoveTo(direction);
    }
}