using System;
using System.Collections.Generic;

namespace func_rocket;

public class LevelsTask
{
	private static readonly Physics standardPhysics = new();

    public static IEnumerable<Level> CreateLevels()
	{
		yield return new Level("Zero", 
			new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
			new Vector(600, 200), 
			(size, v) => Vector.Zero, standardPhysics);

        yield return new Level("Heavy",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            (size, v) => new Vector(0, 0.9), standardPhysics);

        yield return new Level("Up",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(700, 500),
            (size, v) => new Vector(0, -300 / (size.Y - v.Y + 300.0)), standardPhysics);

        yield return new Level("WhiteHole",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            CreateGravity(new Vector(600, 200), -140),
            standardPhysics);

        yield return new Level("BlackHole",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            CreateGravity(new Vector((600 + 200) / 2, (500 + 200) / 2), 300), 
            standardPhysics);

        yield return new Level("BlackAndWhite",
            new Rocket(new Vector(200, 500), Vector.Zero, -0.5 * Math.PI),
            new Vector(600, 200),
            CombineGravity(
                CreateGravity(new Vector(600, 200), -140), 
                CreateGravity(new Vector((600 + 200) / 2, (500 + 200) / 2), 300)
                ), 
            standardPhysics);
    }

    private static Gravity CreateGravity(Vector source, double strengthMultiplier)
    {
        return (size, v) =>
        {
            var distance = (source - v).Length;
            var force = strengthMultiplier * distance / (distance * distance + 1);
            return (source - v).Normalize() * force;
        };
    }

    private static Gravity CombineGravity(params Gravity[] gravities) 
    {
        return (size, v) =>
        {
            Vector result = Vector.Zero;
            foreach (var gravity in gravities)
            {
                result += gravity(size, v);
            }
            return result / gravities.Length;
        };
    }
}