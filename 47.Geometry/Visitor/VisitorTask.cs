using System.Drawing;
using System.IO;

namespace Inheritance.Geometry.Visitor;

public abstract class Body
{
	public Vector3 Position { get; }

	protected Body(Vector3 position)
	{
		Position = position;
	}

	public abstract Body Accept(IVisitor visitor);
}

public class Ball : Body
{
	public double Radius { get; }

	public Ball(Vector3 position, double radius) : base(position)
	{
		Radius = radius;
	}

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class RectangularCuboid : Body
{
	public double SizeX { get; }
	public double SizeY { get; }
	public double SizeZ { get; }

	public RectangularCuboid(Vector3 position, double sizeX, double sizeY, double sizeZ) : base(position)
	{
		SizeX = sizeX;
		SizeY = sizeY;
		SizeZ = sizeZ;
	}

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class Cylinder : Body
{
	public double SizeZ { get; }

	public double Radius { get; }

	public Cylinder(Vector3 position, double sizeZ, double radius) : base(position)
	{
		SizeZ = sizeZ;
		Radius = radius;
	}

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public class CompoundBody : Body
{
	public IReadOnlyList<Body> Parts { get; }

	public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
	{
		Parts = parts;
	}

    public override Body Accept(IVisitor visitor)
    {
        return visitor.Visit(this);
    }
}

public interface IVisitor
{
	Body Visit(Ball ball);
    Body Visit(RectangularCuboid cuboid);
    Body Visit(Cylinder cylinder);
    Body Visit(CompoundBody compoundBody);

}

public class BoundingBoxVisitor : IVisitor
{
    public Body Visit(Ball ball)
    {
        var sizeX = 2 * ball.Radius;
        var sizeY = 2 * ball.Radius;
        var sizeZ = 2 * ball.Radius;

        var position = ball.Position;

        return new RectangularCuboid(position, sizeX, sizeY, sizeZ);
    }

    public Body Visit(RectangularCuboid cuboid)
    {
        return new RectangularCuboid(cuboid.Position, cuboid.SizeX, cuboid.SizeY, cuboid.SizeZ);
    }

    public Body Visit(Cylinder cylinder)
    {
        var sizeX = 2 * cylinder.Radius;
        var sizeY = 2 * cylinder.Radius;
        var sizeZ = cylinder.SizeZ;

        var position = cylinder.Position;

        return new RectangularCuboid(position, sizeX, sizeY, sizeZ);
    }

    public Body Visit(CompoundBody compoundBody)
    {
        var xMin = double.MaxValue; var xMax = double.MinValue;
        var yMin = double.MaxValue; var yMax = double.MinValue;
        var zMin = double.MaxValue; var zMax = double.MinValue;

        foreach (var part in compoundBody.Parts)
        {
            var boundingBox = (RectangularCuboid)part.Accept(this);

            xMin = Math.Min(xMin, boundingBox.Position.X - boundingBox.SizeX / 2);
            yMin = Math.Min(yMin, boundingBox.Position.Y - boundingBox.SizeY / 2);
            zMin = Math.Min(zMin, boundingBox.Position.Z - boundingBox.SizeZ / 2);

            xMax = Math.Max(xMax, boundingBox.Position.X + boundingBox.SizeX / 2);
            yMax = Math.Max(yMax, boundingBox.Position.Y + boundingBox.SizeY / 2);
            zMax = Math.Max(zMax, boundingBox.Position.Z + boundingBox.SizeZ / 2);
        }

        var sizeX = xMax - xMin;
        var sizeY = yMax - yMin;
        var sizeZ = zMax - zMin;

        var position = new Vector3(
            xMin + sizeX / 2,
            yMin + sizeY / 2,
            zMin + sizeZ / 2);

        return new RectangularCuboid(position, sizeX, sizeY, sizeZ);
    }
}

public class BoxifyVisitor : IVisitor
{
    public Body Visit(Ball ball)
    {
        var sizeX = 2 * ball.Radius;
        var sizeY = 2 * ball.Radius;
        var sizeZ = 2 * ball.Radius;

        var position = ball.Position;

        return new RectangularCuboid(position, sizeX, sizeY, sizeZ);
    }

    public Body Visit(RectangularCuboid cuboid)
    {
        return new RectangularCuboid(cuboid.Position, cuboid.SizeX, cuboid.SizeY, cuboid.SizeZ);
    }

    public Body Visit(Cylinder cylinder)
    {
        var sizeX = 2 * cylinder.Radius;
        var sizeY = 2 * cylinder.Radius;
        var sizeZ = cylinder.SizeZ;

        var position = cylinder.Position;

        return new RectangularCuboid(position, sizeX, sizeY, sizeZ);
    }

    public Body Visit(CompoundBody compoundBody)
    {
        return new CompoundBody(compoundBody.Parts
            .Select(part => part.Accept(this))
            .ToList());
    }
}