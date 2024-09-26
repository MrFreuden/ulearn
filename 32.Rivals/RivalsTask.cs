using System;
using System.Collections.Generic;
using System.Linq;

namespace Rivals;

public class RivalsTask
{
	public static IEnumerable<OwnedLocation> AssignOwners(Map map)
	{
        var i = 0;
        var visitedPoints = new HashSet<Point>();
        var chestsPoint = map.Chests.ToDictionary(chest => chest);
        var playersQueue = new Queue<(Point Player, int Index, Queue<Point> PlayerQueue)>(
            map.Players
            .Select(player => 
            {
                var queue = new Queue<Point>();
                queue.Enqueue(player);
                return (Player: player, Index: i++, PlayerQueue: queue);
            } ));

        foreach (var player in playersQueue)
        {
            yield return new OwnedLocation(player.Index, player.Player, 0);
            visitedPoints.Add(player.Player);
        }

        while (playersQueue.Count != 0)
        {
            var currentPlayer = playersQueue.Dequeue();

            var currentQ = new Queue<Point>(currentPlayer.PlayerQueue);
            currentPlayer.PlayerQueue.Clear();

            while (currentQ.Count != 0)
            {
                var point = currentQ.Dequeue();
                foreach (var nextPoint in GetNeighbors(map, point))
                {
                    if (visitedPoints.Contains(nextPoint)) continue;

                    yield return new OwnedLocation(currentPlayer.Index, nextPoint,
                        CalculateDistance(currentPlayer.Player.X, currentPlayer.Player.Y, nextPoint.X, nextPoint.Y));
                    visitedPoints.Add(nextPoint);

                    if (!chestsPoint.TryGetValue(nextPoint, out var chestPoint))
                    {
                        currentPlayer.PlayerQueue.Enqueue(nextPoint);
                    }
                }
            }

            if (currentPlayer.PlayerQueue.Count > 0)
            {
                playersQueue.Enqueue(currentPlayer);
            }
        }
    }


    private static int CalculateDistance(double x1, double y1, double x2, double y2)
    {
        return (int)(Math.Abs(x2 - x1) + Math.Abs(y2 - y1));
    }

    private static IEnumerable<Point> GetNeighbors(Map map, Point current)
    {
        return _possibleDirections
            .Select(x => x + current)
            .Where(point => map.InBounds(point))
            .Where(point => map.Maze[point.X, point.Y] is MapCell.Empty);
    }

    private static List<Point> _possibleDirections = new()
    {
        new Point(0, -1),
        new Point(0, 1),
        new Point(-1, 0),
        new Point(1, 0),
    };
}