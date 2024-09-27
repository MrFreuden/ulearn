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
        var visitedPoints = map.Players.ToHashSet();
        var chestsPoint = map.Chests.ToHashSet();
        var queue = new Queue<OwnedLocation>(map.Players.Select((player, index) => new OwnedLocation(index, player, 0)));

        foreach (var ownedLocation in queue)
            yield return ownedLocation;

        while (queue.Count != 0)
        {
            var currentPoint = queue.Dequeue();

            foreach (var nextPoint in GetNeighbors(map, currentPoint.Location))
            {
                if (visitedPoints.Contains(nextPoint)) continue;

                var ownedLocation = new OwnedLocation(currentPoint.Owner, nextPoint, currentPoint.Distance + 1);
                yield return ownedLocation;

                visitedPoints.Add(nextPoint);

                if (chestsPoint.Contains(nextPoint)) continue;

                queue.Enqueue(ownedLocation);
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
}