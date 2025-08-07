using System.Linq;
using System.Reflection;

namespace MyPhotoshop
{
    public class StaticParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()
    {
        private static readonly PropertyInfo[] _propertyInfos;
        private static readonly ParameterInfo[] _descriptions;

        static StaticParametersHandler()
        {
            _propertyInfos = typeof(TParameters)
               .GetProperties()
               .Where(x => x.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
               .ToArray();

            _descriptions = typeof(TParameters)
                .GetProperties()
                .Select(x => x.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(x => x.Length > 0)
                .Select(x => x[0])
                .Cast<ParameterInfo>()
                .ToArray();
        }

        public ParameterInfo[] GetDescription()
        {
            return _descriptions;
        }

        public TParameters CreateParameters(double[] values)
        {
            var parameters = new TParameters();

            for (int i = 0; i < values.Length; i++)
                _propertyInfos[i].SetValue(parameters, values[i]);
            return parameters;
        }
    }
}
