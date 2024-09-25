using System;
using System.Collections.Generic;
using System.Linq;
namespace Dungeon;

public class DungeonTask
{
    public static MoveDirection[] FindShortestPath(Map map)
    {
        var shortestPath = GetShortestPath(map);

        if (shortestPath == null)
        {
            return FindPathWithoutChest(map);
        }

        var fullPath = shortestPath.Item1.Concat(shortestPath.Item2.Skip(1));

        return GetDirectionsFromPath(fullPath);
    }
    
    private static Tuple<IEnumerable<Point>, SinglyLinkedList<Point>, byte>? GetShortestPath(Map map)
    {
        var combinedPaths = GetCombinedPaths(map);
        return combinedPaths
            .OrderBy(path => path.Item1.Count() + path.Item2.Count())
            .ThenByDescending(way => way.Item3)
            .FirstOrDefault();
    }

    private static IEnumerable<Tuple<IEnumerable<Point>, SinglyLinkedList<Point>, byte>> GetCombinedPaths(Map map)
    {
        var startToChest = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
        var endToChest = BfsTask.FindPaths(map, map.Exit, map.Chests);
        var chestsPoint = map.Chests.ToDictionary(chest => chest.Location, chest => chest);

        var combinedPaths = startToChest.Join(endToChest,
                            startPath => startPath.First(),
                            endPath => endPath.First(),
                                (startPath, endPath) => 
                                Tuple.Create(startPath.Reverse(), endPath, chestsPoint[startPath.First()].Value));

        return combinedPaths;
    }

    private static MoveDirection[] FindPathWithoutChest(Map map)
    {
        var directPath = BfsTask.FindPaths(map, map.Exit, new Chest[] { new(map.InitialPosition, 0) })
                                .FirstOrDefault();
        if (directPath == null) return Array.Empty<MoveDirection>();

        return GetDirectionsFromPath(directPath);
    }

    private static MoveDirection[] GetDirectionsFromPath(IEnumerable<Point> path)
    {
        return path
            .Zip(path.Skip(1), (current, previous) => GetOffset(current, previous))
            .ToArray();
    }

    private static MoveDirection GetOffset(Point current, Point previous)
    {
        if (current == null || previous == null)
            throw new ArgumentNullException(nameof(current));
        return Walker.ConvertOffsetToDirection(new Point(previous.X - current.X, previous.Y - current.Y));
    }
}