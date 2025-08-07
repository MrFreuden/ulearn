namespace Inheritance.Geometry.Virtual;

public abstract class Body
{
    public Vector3 Position { get; }

    protected Body(Vector3 position)
    {
        Position = position;
    }

    public abstract bool ContainsPoint(Vector3 point);

    public abstract RectangularCuboid GetBoundingBox();
}

public class Ball : Body
{
    public double Radius { get; }

    public Ball(Vector3 position, double radius) : base(position)
    {
        Radius = radius;
    }

    public override bool ContainsPoint(Vector3 point)
    {
        var vector = point - Position;
        var length2 = vector.GetLength2();
        return length2 <= Radius * Radius;
    }

    public override RectangularCuboid GetBoundingBox()
    {
        var sizeX = 2 * Radius;
        var sizeY = 2 * Radius;
        var sizeZ = 2 * Radius;

        var position = Position;

        return new RectangularCuboid(position, sizeX, sizeY, sizeZ);
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

    public override bool ContainsPoint(Vector3 point)
    {
        var minPoint = new Vector3(
                Position.X - SizeX / 2,
                Position.Y - SizeY / 2,
                Position.Z - SizeZ / 2);
        var maxPoint = new Vector3(
            Position.X + SizeX / 2,
            Position.Y + SizeY / 2,
            Position.Z + SizeZ / 2);

        return point >= minPoint && point <= maxPoint;
    }

    public override RectangularCuboid GetBoundingBox()
    {
        return new RectangularCuboid(Position, SizeX, SizeY, SizeZ);
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

    public override bool ContainsPoint(Vector3 point)
    {
        var vectorX = point.X - Position.X;
        var vectorY = point.Y - Position.Y;
        var length2 = vectorX * vectorX + vectorY * vectorY;
        var minZ = Position.Z - SizeZ / 2;
        var maxZ = minZ + SizeZ;

        return length2 <= Radius * Radius && point.Z >= minZ && point.Z <= maxZ;
    }

    public override RectangularCuboid GetBoundingBox()
    {
        var sizeX = 2 * Radius;
        var sizeY = 2 * Radius;
        var sizeZ = SizeZ;

        var position = Position;

        return new RectangularCuboid(position, sizeX, sizeY, sizeZ);
    }
}

public class CompoundBody : Body
{
    public IReadOnlyList<Body> Parts { get; }

    public CompoundBody(IReadOnlyList<Body> parts) : base(parts[0].Position)
    {
        Parts = parts;
    }

    public override bool ContainsPoint(Vector3 point)
    {
        return Parts.Any(body => body.ContainsPoint(point));
    }

    public override RectangularCuboid GetBoundingBox()
    {
        var xMin = double.MaxValue;
        var yMin = double.MaxValue;
        var zMin = double.MaxValue;
        var xMax = double.MinValue;
        var yMax = double.MinValue;
        var zMax = double.MinValue;

        foreach (var part in Parts)
        {
            var boundingBox = part.GetBoundingBox();

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