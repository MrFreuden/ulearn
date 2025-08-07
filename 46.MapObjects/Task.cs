namespace Inheritance.MapObjects;

public class Dwelling : IHasOwner
{
	public int Owner { get; set; }
}

public class Mine : IHasArmy, IHasTreasure, IHasOwner
{
	public int Owner { get; set; }
	public Army Army { get; set; }
	public Treasure Treasure { get; set; }
}

public class Creeps : IHasArmy, IHasTreasure
{
	public Army Army { get; set; }
	public Treasure Treasure { get; set; }
}

public class Wolves : IHasArmy
{
	public Army Army { get; set; }
}

public class ResourcePile : IHasTreasure
{
	public Treasure Treasure { get; set; }
}

public interface IHasArmy
{
	Army Army { get; set; }
}

public interface IHasOwner
{
    public int Owner { get; set; }
}

public interface IHasTreasure
{
    public Treasure Treasure { get; set; }
}

public static class Interaction
{
	public static void Make(Player player, object mapObject)
	{
		if (mapObject is null) throw new ArgumentNullException();

		if (mapObject is IHasArmy fighteble)
		{
			if (!player.CanBeat(fighteble.Army))
			{
                player.Die();
                return;
            }
		}
		if (mapObject is IHasOwner capturable)
		{
			capturable.Owner = player.Id;
		}

		if (mapObject is IHasTreasure collectble)
		{
			player.Consume(collectble.Treasure);
		}
	}
}