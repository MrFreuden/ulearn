using System;
using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var chests = state.Chests.ToList();
        var result = Backtrack(state, state.Position, chests, new List<Point>(), (new List<Point>(), Int32.MinValue));
        return result.bestPath;
    }

    public (List<Point> bestPath, int collectedChests) Backtrack(State state, Point position, List<Point> chests, List<Point> fullPath, 
        (List<Point> bestPath, int collectedChests) res)
    {
        if (chests.Count == 0)
        {
            return res;
        }

        var pathFinder = new DijkstraPathFinder();
        var pathWithCost = pathFinder.GetPathsByDijkstra(state, position, chests);
        var costs = pathWithCost.Select(x => x.Cost).ToList();
        

        foreach (var path in pathWithCost)
        {
            var nextMinCost = costs.FirstOrDefault();
            nextMinCost = nextMinCost == 0 ? Int32.MaxValue : nextMinCost;
            if (nextMinCost > state.Energy)
            {
                return res;
            }
            state.Energy -= path.Cost;
            state.Position = path.End;
            chests.Remove(path.End);
            state.Scores++;
            var fullPathCount = fullPath.Count;
            fullPath.AddRange(path.Path.Skip(1));
            costs.RemoveAt(0);
            if (res.collectedChests < state.Scores)
            {
                res.collectedChests = state.Scores;
                res.bestPath = new List<Point>(fullPath);
            }
            res = Backtrack(state, state.Position, chests, fullPath, res);

            state.Energy += path.Cost;
            state.Position = position;
            chests.Add(path.End);
            state.Scores--;
            fullPath.RemoveRange(fullPathCount, path.Path.Count - 1);
        }

        return res;
    }
}
