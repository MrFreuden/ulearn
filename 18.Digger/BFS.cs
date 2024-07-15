using System.Collections;
using System.Collections.Generic;

namespace Digger;

public class Node
{
    public int Y { get; set; }
    public int X { get; set; }
    public int Distance { get; set; }
    public Node? Parent { get; set; }
}
public static class BFS
{
    private static bool[][] FillVisited()
    {
        var visited = new bool[Game.MapHeight][];
        for (int y = 0; y < Game.MapHeight; y++)
        {
            visited[y] = new bool[Game.MapWidth];
            for (int x = 0; x < Game.MapWidth; x++)
            {
                visited[y][x] = Game.Map[x, y] is IMovementBlockerMonster;
            }
        }
        return visited;
    }

    public static Node? Bfs(Node start)
    {
        var visited = FillVisited();
        var queue = new Queue<Node>();
        queue.Enqueue(start);
        visited[start.Y][start.X] = true;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (Game.Map[current.X, current.Y] is Player && current.Distance != 0)
            {
                return current;
            }

            EnqueueIfValid(current.Y - 1, current.X, current, visited, queue);
            EnqueueIfValid(current.Y + 1, current.X, current, visited, queue);
            EnqueueIfValid(current.Y, current.X - 1, current, visited, queue);
            EnqueueIfValid(current.Y, current.X + 1, current, visited, queue);
        }
        return null;
    }

    private static void EnqueueIfValid(int x, int y, Node current, bool[][] visited, Queue<Node> queue)
    {
        if (GameFieldHelper.IsWithinMapBounds(x, y) && !visited[x][y])
        {
            queue.Enqueue(new Node { Y = x, X = y, Distance = current.Distance + 1, Parent = current });
            visited[x][y] = true;
        }
    }
}
