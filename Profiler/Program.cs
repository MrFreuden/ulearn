using MyPhotoshop;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Profiler
{
    internal class Program
    {
        public static void Test(Func<double[], LighteningParameters> action, int N)
        {
            var args = new double[] { 0 };
            action(args);

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < N; i++)
            {
                action(args);
            }
            sw.Stop();
            Console.WriteLine(1000 * (double)sw.ElapsedMilliseconds / N);
        }

        static void Main(string[] args)
        {
            var handler = new SimpleParametersHandler<LighteningParameters>();
            Test(values => handler.CreateParameters(values), 1000000);

            var statHandler = new StaticParametersHandler<LighteningParameters>();
            Test(values => statHandler.CreateParameters(values), 1000000);

            var expressionHandler = new ExpressionParametersHandler<LighteningParameters>();
            Test(values => expressionHandler.CreateParameters(values), 1000000);

            Test(values => new LighteningParameters { Coefficient = values[0] }, 1000000);        
        }
    }
}
