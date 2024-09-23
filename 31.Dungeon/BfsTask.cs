using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
    private static readonly List<Point> _direction = new()
    {
        new Point(1, 0),
        new Point(-1, 0),
        new Point(0, 1),
        new Point(0, -1)
    };

    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
    {
        var visited = new HashSet<Point>() { start };
        var chestsPoint = new HashSet<Point>(chests.Select(chest => chest.Location));
        var queue = new Queue<SinglyLinkedList<Point>>();
        queue.Enqueue(new SinglyLinkedList<Point>(start));

        while (queue.Count != 0)
        {
            var node = queue.Dequeue();

            foreach (var nextNode in GetNeighbors(map, node))
            {
                if (visited.Add(nextNode.Value))
                {
                    queue.Enqueue(nextNode);
                    if (chestsPoint.Contains(nextNode.Value))
                    {
                        yield return nextNode;
                    }
                }
            }
        }
    }

    private static IEnumerable<SinglyLinkedList<Point>> GetNeighbors(Map map, SinglyLinkedList<Point> current)
    {
        return _direction
            .Select(x => new SinglyLinkedList<Point>(x + current.Value, current))
            .Where(point => map.InBounds(point.Value) && !point.Value.Equals(current.Previous?.Value))
            .Where(point => map.Dungeon[point.Value.X, point.Value.Y] is MapCell.Empty);
    }
}