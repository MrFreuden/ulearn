using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
	public Rocket GetNextMove(Rocket rocket)
	{
        var iterationsPerThread = iterationsCount / threadsCount;
        var tasks = new List<Task<(Turn Turn, double Score)>>();
        for (int i = 0; i < threadsCount; i++)
        {
            lock (random)
            {
                tasks.Add(Task.Run(() => SearchBestMove(rocket, new Random(random.Next()), iterationsPerThread)));
            }
        }

        Task.WaitAll(tasks.ToArray());

        var bestMove = tasks
            .Select(x => x.Result)
            .OrderByDescending(x => x.Score)
            .First();
		
		return rocket.Move(bestMove.Turn, level);
	}
	
	public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
	{
		return new() { Task.Run(() => SearchBestMove(rocket, new Random(random.Next()), iterationsCount)) };
	}
}