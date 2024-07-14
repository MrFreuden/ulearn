using Digger.Architecture;

namespace Digger;

public class Terrain : ICreature, IMovementBlockerMonster
{
    public CreatureCommand Act(int x, int y) => new ();

    public bool DeadInConflict(ICreature conflictedObject) => true;

    public int GetDrawingPriority() => 1;

    public string GetImageFileName() => "Terrain.png";
}
