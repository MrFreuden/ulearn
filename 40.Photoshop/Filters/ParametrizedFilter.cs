namespace MyPhotoshop
{
    public abstract class ParametrizedFilter<TParameters> : IFilter
        where TParameters : IParameters, new()
    {
        private readonly IParametersHandler<TParameters> _handler = new ExpressionParametersHandler<TParameters>();

        public ParameterInfo[] GetParameters()
        {
            return _handler.GetDescription();
        }

        public Photo Process(Photo original, double[] values)
        {
            var parameters = _handler.CreateParameters(values);
            return Process(original, parameters);
        }

        public abstract Photo Process(Photo photo, TParameters parameters);
    }
}
