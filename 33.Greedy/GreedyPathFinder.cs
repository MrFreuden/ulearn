using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
	public List<Point> FindPathToCompleteGoal(State state)
	{
		if (state.Chests.Count == 0 || state.InitialEnergy == 0 || state.Goal == 0)
			return new List<Point>();

		var chests = state.Chests;
		var pathFinder = new DijkstraPathFinder();
		var fullPath = new List<Point>();
		while (state.Scores != state.Goal)
		{
			var pathWithCost = pathFinder.GetPathsByDijkstra(state, state.Position, chests);
			if (!pathWithCost.Any())
            {
				return new List<Point>();
            }
            var path = pathWithCost.First().Path;
			var cost = pathWithCost.First().Cost;
            path.RemoveAt(0);
			if (state.Energy - cost < 0)
			{
                return new List<Point>();
            }
			state.Energy -= cost;
            state.Position = path.Last();
			fullPath.AddRange(path);
			chests.Remove(path.Last());
			state.Scores++;

        }
		
		return fullPath;
	}
}