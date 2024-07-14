using Digger.Architecture;
using System;

namespace Digger;

public class Sack : ICreature, IMovementBlockerPlayer, IMovementBlockerMonster
{
    private int _fallDistance;

    public CreatureCommand Act(int x, int y)
    {
        if (ShouldFall(x, y))
        {
            _fallDistance++;
            return new CreatureCommand { DeltaY = 1 };
        }
        if (ShouldTransformToGold())
        {
            return new CreatureCommand { TransformTo = new Gold() };
        }
        _fallDistance = 0;
        return new CreatureCommand();
    }

    private bool ShouldFall(int x, int y)
    {
        return GameFieldHelper.IsCreatureOrEmptyBelow(x, y, null) ||
        (_fallDistance > 0 && (GameFieldHelper.IsCreatureOrEmptyBelow(x, y, typeof(Player)) ||
        GameFieldHelper.IsCreatureOrEmptyBelow(x, y, typeof(Monster))));
    }

    private bool ShouldTransformToGold() => _fallDistance > 1;
  
    public bool DeadInConflict(ICreature conflictedObject) => false;

    public int GetDrawingPriority() => 1;

    public string GetImageFileName() => "Sack.png";
}
