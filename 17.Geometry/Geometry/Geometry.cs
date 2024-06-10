namespace GeometryTasks
{
    public class Geometry
    {
        public static double GetLength(Vector vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static double GetLength(Segment segment)
        {
            return Math.Sqrt(Math.Pow(segment.End.X - segment.Begin.X, 2) + Math.Pow(segment.End.Y - segment.Begin.Y, 2));
        }

        public static Vector Add(Vector vector1, Vector vector2)
        {
            return new Vector { X = vector1.X + vector2.X, Y = vector1.Y + vector2.Y };
        }

        public static bool IsVectorInSegment(Vector vector, Segment segment)
        {
            var distance = Math.Sqrt(Math.Pow(segment.Begin.X - vector.X, 2) + Math.Pow(segment.Begin.Y - vector.Y, 2));
            distance += Math.Sqrt(Math.Pow(segment.End.X - vector.X, 2) + Math.Pow(segment.End.Y - vector.Y, 2));
            return distance == GetLength(segment);
        } 
    }
}
