namespace Mazes;

public static class EmptyMazeTask
{
	public static void MoveOut(Robot robot, int width, int height)
	{
        int targetX = width - 2;
        int targetY = height - 2;
        MoveRight(robot, targetX - robot.X);
        MoveDown(robot, targetY - robot.Y);
    }

    private static void MoveDown(Robot robot, int stepCount)
    {
        for (int i = 0; i < stepCount; i++)
            robot.MoveTo(Direction.Down);
    }

    private static void MoveRight(Robot robot, int stepCount)
	{
        for (int i = 0; i < stepCount; i++)
            robot.MoveTo(Direction.Right);
    }
}