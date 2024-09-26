using System;
using System.Collections.Generic;
using System.Linq;

namespace Rivals;

public class RivalsTask
{
    private readonly static List<Point> _possibleDirections = new()
    {
        new Point(0, -1),
        new Point(0, 1),
        new Point(-1, 0),
        new Point(1, 0),
    };
    
    public static IEnumerable<OwnedLocation> AssignOwners(Map map)
	{
        var visitedPoints = new HashSet<Point>();
        var chestsPoint = map.Chests.ToHashSet();
        var playersQueue = InitializePlayersQueue(map.Players);

        foreach (var player in playersQueue)
        {
            yield return new OwnedLocation(player.Index, player.Player, 0);
            visitedPoints.Add(player.Player);
        }

        while (playersQueue.Count != 0)
        {
            var currentPlayer = playersQueue.Dequeue();
            var count = currentPlayer.PlayerQueue.Count;

            for (int i = 0; i < count; i++)
            {
                foreach (var ownedLocation in ProcessNeighbors(map, currentPlayer, visitedPoints, chestsPoint))
                {
                    yield return ownedLocation;
                }
            }

            if (currentPlayer.PlayerQueue.Count > 0)
            {
                playersQueue.Enqueue(currentPlayer);
            }
        }
    }

    private static Queue<(Point Player, int Index, Queue<Point> PlayerQueue)> InitializePlayersQueue(Point[] players)
    {
        return new Queue<(Point Player, int Index, Queue<Point> PlayerQueue)>(
            players
            .Select((player, index) =>
            {
                var queue = new Queue<Point>();
                queue.Enqueue(player);
                return (Player: player, Index: index, PlayerQueue: queue);
            })
            );
    }

    private static IEnumerable<OwnedLocation> ProcessNeighbors(
        Map map,
        (Point Player, int Index, Queue<Point> PlayerQueue) currentPlayer,
        HashSet<Point> visitedPoints,
        HashSet<Point> chestsPoint)
    {
        var point = currentPlayer.PlayerQueue.Dequeue();
        foreach (var nextPoint in GetNeighbors(map, point))
        {
            if (visitedPoints.Contains(nextPoint)) continue;

            yield return new OwnedLocation(currentPlayer.Index, nextPoint,
                CalculateDistance(currentPlayer.Player.X, currentPlayer.Player.Y, nextPoint.X, nextPoint.Y));

            visitedPoints.Add(nextPoint);

            if (!chestsPoint.Contains(nextPoint))
            {
                currentPlayer.PlayerQueue.Enqueue(nextPoint);
            }
        }
    }

    private static IEnumerable<Point> GetNeighbors(Map map, Point current)
    {
        return _possibleDirections
            .Select(point => point + current)
            .Where(point => map.InBounds(point))
            .Where(point => map.Maze[point.X, point.Y] is MapCell.Empty);
    }

    private static int CalculateDistance(double x1, double y1, double x2, double y2)
    {
        return (int)(Math.Abs(x2 - x1) + Math.Abs(y2 - y1));
    }
}