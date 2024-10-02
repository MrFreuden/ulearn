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
        var result = Backtrack(state, state.Position, chests, new List<Point>(), new DijkstraPathFinder(), (new List<Point>(), Int32.MinValue));
        return result.bestPath;
    }

    public (List<Point> bestPath, int collectedChests) Backtrack(
        State state, 
        Point position, 
        List<Point> chests, 
        List<Point> fullPath, 
        DijkstraPathFinder pathFinder,
        (List<Point> bestPath, int collectedChests) result
        )
    {
        if (chests.Count == 0
            || state.Scores + chests.Count < result.collectedChests || state.Energy <= 0)
        {
            return result;
        }

        var pathsWithCost = pathFinder.GetPathsByDijkstra(state, position, chests);
        
        foreach (var pathWithCost in pathsWithCost)
        {

            if (pathWithCost.Cost > state.Energy 
                || state.Scores + chests.Count < result.collectedChests)
            {
                break;
            }

            UpdateStateForPath(state, pathWithCost);
            
            chests.Remove(pathWithCost.End);

            var fullPathCount = fullPath.Count;
            fullPath.AddRange(pathWithCost.Path.Skip(1));

            if (result.collectedChests < state.Scores)
            {
                result.collectedChests = state.Scores;
                result.bestPath = new List<Point>(fullPath);
            }

            result = Backtrack(state, state.Position, chests, fullPath, pathFinder, result);

            RestoreStateAfterPath(state, pathWithCost, position);
            
            chests.Add(pathWithCost.End);

            fullPath.RemoveRange(fullPathCount, pathWithCost.Path.Count - 1);
            if (result.collectedChests == chests.Count)
            {
                return result;
            }
        }

        return result;
    }

    private void UpdateStateForPath(State state, PathWithCost path)
    {
        state.Energy -= path.Cost;
        state.Position = path.End;
        state.Scores++;
    }

    private void RestoreStateAfterPath(State state, PathWithCost path, Point position)
    {
        state.Energy += path.Cost;
        state.Position = position;
        state.Scores--;
    }
}
