using Digger.Architecture;

namespace Digger;

public class Monster : ICreature, IMovementBlockerMonster
{
    public CreatureCommand Act(int x, int y)
    {
        var nextPosition = FindWay(x, y);
        var deltaX = nextPosition.X - x;
        var deltaY = nextPosition.Y - y;

        return new CreatureCommand() { DeltaX = deltaX, DeltaY = deltaY };
    }

    private Node FindWay(int x, int y)
    {
        var node = BFS.Bfs(new Node { X = x, Y = y });
        if (node == null)
        {
            return new Node { X = x, Y = y };
        }
        while (node.Parent.Parent != null)
        {
            node = node.Parent;
        }
        return node;
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        if (conflictedObject is Monster || conflictedObject is Sack)
        {
            return true;
        }

        return false;
    }

    public int GetDrawingPriority() => 5;
    
    public string GetImageFileName() => "Monster.png";
}
