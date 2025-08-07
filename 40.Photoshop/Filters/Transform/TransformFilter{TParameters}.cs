using System.Drawing;

namespace MyPhotoshop
{
    public class TransformFilter<TParameters> : ParametrizedFilter<TParameters>
        where TParameters : IParameters, new()
    {
        private readonly string _name;
        private readonly ITransformer<TParameters> _transformer;

        public TransformFilter(string name, ITransformer<TParameters> transformer)
        {
            _name = name;
            _transformer = transformer;
        }

        public override string ToString()
        {
            return _name;
        }

        public override Photo Process(Photo photo, TParameters parameters)
        {
            var oldSize = new Size(photo.Width, photo.Height);
            _transformer.Prepare(oldSize, parameters);
            var result = new Photo(_transformer.ResultSize.Width, _transformer.ResultSize.Height);
            for (int x = 0; x < result.Width; x++)
            {
                for (int y = 0; y < result.Height; y++)
                {
                    var point = new Point(x, y);
                    var oldPoint = _transformer.MapPoint(point);
                    if (oldPoint.HasValue)
                        result[x, y] = photo[oldPoint.Value.X, oldPoint.Value.Y];
                }
            }
            return result;
        }
    }
}
