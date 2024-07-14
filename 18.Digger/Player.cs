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
        if (IsAvailbleMove(x + deltaX, y + deltaY))
            return new CreatureCommand { DeltaX = deltaX, DeltaY = deltaY };
        return new CreatureCommand();
    }

    private bool IsAvailbleMove(int newX, int newY)
    {
        return GameFieldHelper.IsWithinMapBounds(newX, newY) && !GameFieldHelper.IsMovementBlockerPlayerOnWay(newX, newY);
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

    public int GetDrawingPriority() => 5;

    public string GetImageFileName() => "Digger.png";
}
