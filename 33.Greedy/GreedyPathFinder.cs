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
			var path = pathWithCost.FirstOrDefault();

			if (path == default || state.Energy - path.Cost < 0) return new List<Point>(); 

			state.Energy -= path.Cost;
            state.Position = path.End;
			fullPath.AddRange(path.Path.Skip(1));
			chests.Remove(path.End);
			state.Scores++;
        }
		return fullPath;
	}
}