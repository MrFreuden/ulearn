namespace Mazes;

public static class EmptyMazeTask
{
	public static void MoveOut(Robot robot, int width, int height)
	{
		while (robot.X != width - 2 || robot.Y != height - 2)
		{
            if (width - robot.X > height - robot.Y)
				robot.MoveTo(Direction.Right);
			else
				robot.MoveTo(Direction.Down);
		}
		while (robot.X != width - 2)
            robot.MoveTo(Direction.Right);
		while (robot.Y != height - 2)
			robot.MoveTo(Direction.Down);
    }
}