using System;
using System.Drawing;

namespace MyPhotoshop
{
    public class FreeTransformer : ITransformer<EmptyParameters>
    {
        Func<Size, Size> _sizeTransform;
        Func<Point, Size, Point> _pointTransform;
        private Size _oldSize;

        public FreeTransformer(Func<Size, Size> sizeTransform, Func<Point, Size, Point> pointTransform)
        {
            _sizeTransform = sizeTransform;
            _pointTransform = pointTransform;
        }

        public Size ResultSize { get; private set; }

        public Point? MapPoint(Point newPoint)
        {
            return _pointTransform(newPoint, _oldSize);
        }

        public void Prepare(Size size, EmptyParameters parameters)
        {
            _oldSize = size;
            ResultSize = _sizeTransform(size);
        }
    }
}
