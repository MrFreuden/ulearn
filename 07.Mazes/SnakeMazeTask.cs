namespace Mazes;

public static class SnakeMazeTask
{
    public static void MoveOut(Robot robot, int width, int height)
    {
        int count = height - 2 - robot.Y;
        for (int i = 0; i < count / 2; i++)
        {
            MoveRight(robot, width - 3);
            MoveDown(robot, 2);
            MoveLeft(robot, width - 3);
            if (robot.Finished)
                break;
            MoveDown(robot, 2);
        }
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

    private static void MoveLeft(Robot robot, int stepCount)
    {
        for (int i = 0; i < stepCount; i++)
            robot.MoveTo(Direction.Left);
    }
}