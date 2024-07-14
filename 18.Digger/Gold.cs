using Digger.Architecture;

namespace Digger;

public class Gold : ICreature
{
    public CreatureCommand Act(int x, int y) => new ();

    public bool DeadInConflict(ICreature conflictedObject) => true;

    public int GetDrawingPriority() => 1;

    public string GetImageFileName() => "Gold.png";
}
