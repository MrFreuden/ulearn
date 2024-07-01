using System;
using Avalonia.Input;
using Digger.Architecture;

namespace Digger;

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
        if (IsAvaibleMove(x + deltaX, y + deltaY)) 
            return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY };
        return new CreatureCommand();
    }

    private bool IsAvaibleMove(int newX, int newY)
    {
        return newX >= 0 && newX < Game.MapWidth && newY >= 0 && newY < Game.MapHeight;
    }

    public bool DeadInConflict(ICreature conflictedObject)
    {
        if (conflictedObject is Terrain)
        {
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