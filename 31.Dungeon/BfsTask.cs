using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Chest[] chests)
    {
        var visited = new HashSet<Point>();
        var queue = new Queue<SinglyLinkedList<Point>>();
        var startLink = new SinglyLinkedList<Point>(start);
        visited.Add(start);
        queue.Enqueue(startLink);
        while (queue.Count != 0)
        {
            var node = queue.Dequeue();

            foreach (var nextNode in GetNeibors(map, node))
            {
                if (!visited.Contains(nextNode.Value))
                {
                    visited.Add(nextNode.Value);
                    queue.Enqueue(nextNode);
                    if (chests.Any(x => x.Location == nextNode.Value))
                    {
                        yield return nextNode;
                    }
                }
            }
        }
    }

    private static IEnumerable<SinglyLinkedList<Point>> GetNeibors(Map map, SinglyLinkedList<Point> current)
    {
        var points = new List<SinglyLinkedList<Point>>()
        {
            new(new Point(current.Value.X + 1, current.Value.Y), current),
            new(new Point(current.Value.X - 1, current.Value.Y), current),
            new(new Point(current.Value.X, current.Value.Y + 1), current),
            new(new Point(current.Value.X, current.Value.Y - 1), current)
        };

        return points
            .Where(point => map.InBounds(point.Value) && point.Value != current.Previous?.Value)
            .Where(point => map.Dungeon[point.Value.X, point.Value.Y] is MapCell.Empty);
    }
}