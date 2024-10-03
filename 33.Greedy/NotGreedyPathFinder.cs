using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var chests = state.Chests.ToList();
        var (bestPath, _) = Backtrack(state, state.Position, chests, 
                                            new List<Point>(), new DijkstraPathFinder(), (new List<Point>(), Int32.MinValue));
        return bestPath;
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
        if (ShouldTerminateBacktrack(state, chests, result))
        {
            return result;
        }

        var pathsWithCost = pathFinder.GetPathsByDijkstra(state, position, chests);
        
        foreach (var pathWithCost in pathsWithCost)
        {
            if (ShouldSkipPath(pathWithCost, state)) break;

            ProcessPath(state, pathWithCost, chests, fullPath);
            
            result = TryUpdateResult(state, fullPath, result);

            result = Backtrack(state, state.Position, chests, fullPath, pathFinder, result);

            RestoreStateAfterPath(state, pathWithCost, chests, fullPath);
        }

        return result;
    }

    private bool ShouldTerminateBacktrack(State state, List<Point> chests, (List<Point> bestPath, int collectedChests) result) => 
        chests.Count == 0 || state.Scores + chests.Count <= result.collectedChests;
    
    private bool ShouldSkipPath(PathWithCost pathWithCost, State state) => pathWithCost.Cost > state.Energy;

    private void ProcessPath(State state, PathWithCost pathWithCost, List<Point> chests, List<Point> fullPath)
    {
        UpdateStateForPath(state, pathWithCost);
        chests.Remove(pathWithCost.End);
        fullPath.AddRange(pathWithCost.Path.Skip(1));
    }

    private void UpdateStateForPath(State state, PathWithCost path)
    {
        state.Energy -= path.Cost;
        state.Position = path.End;
        state.Scores++;
    }

    private (List<Point> bestPath, int collectedChests) TryUpdateResult(State state, List<Point> fullPath, (List<Point> bestPath, int collectedChests) result)
    {
        if (result.collectedChests < state.Scores)
        {
            result.collectedChests = state.Scores;
            result.bestPath = new List<Point>(fullPath);
        }
        return result;
    }

    private void RestoreStateAfterPath(State state, PathWithCost pathWithCost, List<Point> chests, List<Point> fullPath)
    {
        chests.Add(pathWithCost.End);
        fullPath.RemoveRange(fullPath.Count - (pathWithCost.Path.Count - 1), pathWithCost.Path.Count - 1);
        RestoreStateAfterPath(state, pathWithCost);
    }

    private void RestoreStateAfterPath(State state, PathWithCost path)
    {
        state.Energy += path.Cost;
        state.Position = path.Start;
        state.Scores--;
    }
}
