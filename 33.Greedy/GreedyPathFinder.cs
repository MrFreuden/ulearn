using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
	public List<Point> FindPathToCompleteGoal(State state)
	{
		if (state.Chests.Count == 0 || state.InitialEnergy == 0 ||state.Goal == 0)
			return new List<Point>();
		var s = state.Scores;
		var chests = state.Chests;
		var goal = state.Goal;
		var pathFinder = new DijkstraPathFinder();
		var fullPath = new List<Point>();
		while (goal != 0)
		{
            var pathWithCost = pathFinder.GetPathsByDijkstra(state, state.Position, chests).First();
            var path = pathWithCost.Path;
			var cost = pathWithCost.Cost;
			chests.Remove(path.Last());
            if (state.Energy < cost)
            {
                return new List<Point>();
            }
            path.RemoveAt(0);
			state.Energy -= cost;
            
            state.Position = path.Last();
			fullPath.AddRange(path);
			goal--;
            if (chests.Count == 0 && goal > 0)
            {
                return new List<Point>();
            }
        }
		
		return fullPath;
	}
}