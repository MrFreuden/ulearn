using System;
using System.Collections.Generic;
using System.Linq;
namespace Dungeon;

public class DungeonTask
{
    public static MoveDirection[] FindShortestPath(Map map)
    {
        var chestsPoint = map.Chests.ToDictionary(chest => chest.Location, chest => chest);
        
        var startToChest = BfsTask.FindPaths(map, map.InitialPosition, chestsPoint.Values.ToArray());

        var endToChest = BfsTask.FindPaths(map, map.Exit, chestsPoint.Values.ToArray());

        var combinedPaths = startToChest.Join(endToChest,
                            startPath => startPath.First(),
                            endPath => endPath.First(),
                                (startPath, endPath) => new
                                {
                                    MidToStart = startPath.Reverse(),
                                    MidToEnd = endPath,
                                    ChestValue = chestsPoint[startPath.First()].Value
                                });

        var shortestPath = combinedPaths
            .OrderBy(path => path.MidToStart.Count() + path.MidToEnd.Count())
            .ThenByDescending(way => way.ChestValue)
            .FirstOrDefault();

        if (shortestPath == null || !startToChest.Any()|| !endToChest.Any())
        {
            return FindPathWithoutChest(map);
        }

        var fullPath = shortestPath.MidToStart.Concat(shortestPath.MidToEnd.Skip(1));

        return GetDirectionsFromPath(fullPath);
    }
    private static MoveDirection[] GetDirectionsFromPath(IEnumerable<Point> path)
    {
        return path.Zip(path.Skip(1), (current, previous) => GetOffset(current, previous))
            .ToArray();
    }

    private static MoveDirection[] FindPathWithoutChest(Map map)
    {
        var directPath = BfsTask.FindPaths(map, map.Exit, new Chest[] { new Chest(map.InitialPosition, 0) })
                                .FirstOrDefault();
        if (directPath == null) return new MoveDirection[0];

        return GetDirectionsFromPath(directPath);
    }

    private static MoveDirection GetOffset(Point current, Point? previous)
    {
        return Walker.ConvertOffsetToDirection(new Point(previous.X - current.X, previous.Y - current.Y));
    }
}