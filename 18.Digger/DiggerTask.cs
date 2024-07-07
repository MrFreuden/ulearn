using Avalonia.Input;
using Digger.Architecture;
using System;

namespace Digger;

public interface IMovementBlocker { }

public class Player : ICreature
{
    public CreatureCommand Act(int x, int y)
    {
        int deltaX = 0;
        int deltaY = 0;
        switch (Game.KeyPressed)
        {
            case Key.Left: deltaX = -1; break;
            case Key.Up: deltaY = -1; break;
            case Key.Right: deltaX = 1; break;
            case Key.Down: deltaY = 1; break;
        }
        if (IsAvailbleMove(x + deltaX, y + deltaY))
            return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY };
        return new CreatureCommand();
    }

    private bool IsAvailbleMove(int newX, int newY)
    {
        return IsMoveWithinTheMap(newX, newY) && !IsMovementBlockerOnWay(newX, newY);
    }

    private bool IsMoveWithinTheMap(int newX, int newY)
    {
        return newX >= 0 && newX < Game.MapWidth && newY >= 0 && newY < Game.MapHeight;
    }

    private bool IsMovementBlockerOnWay(int newX, int newY)
    {
        return Game.Map[newX, newY] is IMovementBlocker;
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        if (conflictedObject is Terrain)
        {
            return false;
        }
        if (conflictedObject is Gold)
        {
            Game.Scores += 10;
            return false;
        }
        return true;
    }

    public int GetDrawingPriority()
    {
        return 5;
    }

    public string GetImageFileName()
    {
        return "Digger.png";
    }
}

public class Terrain : ICreature
{
    public CreatureCommand Act(int x, int y)
    {
        return new CreatureCommand();
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        return true;
    }

    public int GetDrawingPriority()
    {
        return 1;
    }

    public string GetImageFileName()
    {
        return "Terrain.png";
    }
}

public class Sack : ICreature, IMovementBlocker
{
    private const int FallStep = 1;
    private int fallDistance;
    public CreatureCommand Act(int x, int y)
    {
        if (ShouldFall(x, y))
        {
            fallDistance++;
            return new CreatureCommand() { DeltaY = FallStep };
        }
        else if (ShouldTransformToGold())
        {
            return new CreatureCommand() { TransformTo = new Gold() };
        }
        fallDistance = 0;
        return new CreatureCommand();
    }

    private bool ShouldTransformToGold()
    {
        return fallDistance > 1;
    }

    private bool ShouldFall(int x, int y)
    {
        return IsCreatureOrEmptyBelow(x, y, null) || (fallDistance > 0 && IsCreatureOrEmptyBelow(x, y, typeof(Player)));
    }

    private bool IsWithinMapBounds(int x, int y)
    {
        return x >= 0 && x < Game.MapWidth && y >= 0 && y < Game.MapHeight;
    }

    private bool IsCreatureOrEmptyBelow(int x, int y, Type? creatureType)
    {
        if (IsWithinMapBounds(x, y + 1))
        {
            return Game.Map[x, y + 1]?.GetType() == creatureType;
        }
        return false;
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        return false;
    }

    public int GetDrawingPriority()
    {
        return 1;
    }

    public string GetImageFileName()
    {
        return "Sack.png";
    }
}

public class Gold : ICreature
{
    public CreatureCommand Act(int x, int y)
    {
        return new CreatureCommand();
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        return true;
    }

    public int GetDrawingPriority()
    {
        return 1;
    }

    public string GetImageFileName()
    {
        return "Gold.png";
    }
}