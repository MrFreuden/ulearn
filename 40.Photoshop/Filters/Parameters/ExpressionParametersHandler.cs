using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MyPhotoshop
{
    public class ExpressionParametersHandler<TParameters> : IParametersHandler<TParameters>
        where TParameters : IParameters, new()
    {
        private static readonly ParameterInfo[] _descriptions;
        private static readonly Func<double[], TParameters> _parser;
        static ExpressionParametersHandler()
        {
            _descriptions = typeof(TParameters)
                .GetProperties()
                .Select(x => x.GetCustomAttributes(typeof(ParameterInfo), false))
                .Where(x => x.Length > 0)
                .Select(x => x[0])
                .Cast<ParameterInfo>()
                .ToArray();
           



            var propertyInfos = typeof(TParameters)
              .GetProperties()
              .Where(x => x.GetCustomAttributes(typeof(ParameterInfo), false).Length > 0)
              .ToArray();

            var arg = Expression.Parameter(typeof(double[]), "values");
            var bindings = new List<MemberBinding>();
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                bindings.Add(Expression.Bind(
                    propertyInfos[i],
                    Expression.ArrayIndex(
                        arg,
                        Expression.Constant(i))));
            }
            var body = Expression.MemberInit(
                Expression.New(typeof(TParameters).GetConstructor(new Type[0])),
                bindings
                );
            var lambda = Expression.Lambda<Func<double[], TParameters>>(
                body,
                arg);
            _parser = lambda.Compile();

        }

        public ParameterInfo[] GetDescription()
        {
            return _descriptions;
        }

        public TParameters CreateParameters(double[] values)
        {
            return _parser(values);
        }
    }
}
