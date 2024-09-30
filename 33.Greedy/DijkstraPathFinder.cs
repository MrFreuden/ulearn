using DynamicData;
using Greedy.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Greedy;

public class DijkstraPathFinder
{
    private class DijkstraData
    {
        public Point? Previous { get; set; }
        public double Price { get; set; }
    }

    private static readonly List<Point> _directions = new()
            {
                new Point(1, 0),
                new Point(-1, 0),
                new Point(0, 1),
                new Point(0, -1),
            };

    public IEnumerable<PathWithCost> GetPathsByDijkstra(
        State state,
        Point start,
        IEnumerable<Point> targets)
    {
        var targetsSet = new HashSet<Point>(targets);
        var visited = new HashSet<Point>();
        var track = new Dictionary<Point, DijkstraData>
        {
            [start] = new DijkstraData { Price = 0, Previous = null }
        };

        var queue = new PriorityQueue<Point, double>();
        queue.Enqueue(start, 0.0);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (!visited.Add(current)) continue;

            if (targetsSet.Contains(current))
            {
                var path = BuildPath(track, current);
                yield return new PathWithCost((int)track[current].Price, path.ToArray());
                targetsSet.Remove(current);
                if (targetsSet.Count == 0) yield break;
            }

            foreach (var e in GetIncidentEdges(state, current))
            {
                if (visited.Contains(e)) continue;
                var currentPrice = track[current].Price + state.CellCost[e.X, e.Y];
                if (!track.ContainsKey(e) || track[e].Price > currentPrice)
                {
                    track[e] = new DijkstraData { Previous = current, Price = currentPrice };
                    queue.Enqueue(e, currentPrice);
                }
            }
        }
    }

    private List<Point> BuildPath(Dictionary<Point, DijkstraData> track, Point currentNode)
    {
        var result = new List<Point>();
        Point? current = currentNode;
        while (current != null)
        {
            result.Add(current.Value);
            current = track[current.Value].Previous;
        }
        result.Reverse();
        return result;
    }

    private IEnumerable<Point> GetIncidentEdges(State state, Point currentPoint)
    {
        return _directions
            .Select(point => currentPoint + point)
            .Where(point => state.InsideMap(point) && !state.IsWallAt(point));
    }
}

