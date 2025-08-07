using System.Linq;
using System.Reflection;

namespace MyPhotoshop
{
    public class SimpleParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()
    {
        public ParameterInfo[] GetDescription()
        {
            return typeof(TParameters)
                .GetProperties()
                .Select(x => x.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(x => x.Length > 0)
                .Select(x => x[0])
                .Cast<ParameterInfo>()
                .ToArray();
        }

        public TParameters CreateParameters(double[] values)
        {
            var parameters = new TParameters();
            var props = typeof(TParameters)
               .GetProperties()
               .Where(x => x.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
               .ToArray();

            for (int i = 0; i < values.Length; i++)
                props[i].SetValue(parameters, values[i]);
            return parameters;
        }
    }
}
