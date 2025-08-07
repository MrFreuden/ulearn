using System.Linq.Expressions;

namespace Reflection.Differentiation;

public static class Algebra
{
    private static readonly Dictionary<
        ExpressionType, 
        Func<Expression, ParameterExpression, Expression>> _diffFuncs = new();
    private static readonly HashSet<string> _supportedMethods = new() { "Sin", "Cos" };
    static Algebra()
    {
        InitializeExprDiffFuncs();
    }

    private static void InitializeExprDiffFuncs()
    {
        _diffFuncs.Add(
            ExpressionType.Add,
            (expArg, param) =>
            {
                var binaryExp = (BinaryExpression)expArg;
                var leftDerivative = DifferentiateExpression(binaryExp.Left, param);
                var rightDerivative = DifferentiateExpression(binaryExp.Right, param);
                return Expression.Add(leftDerivative, rightDerivative);
            });

        _diffFuncs.Add(
            ExpressionType.Multiply,
            (expArg, param) =>
            {
                var binaryExp = (BinaryExpression)expArg;

                if (binaryExp.Left is ConstantExpression || binaryExp.Right is ConstantExpression)
                {
                    var constExpr = binaryExp.Left as ConstantExpression ?? binaryExp.Right as ConstantExpression;
                    var otherExpr = binaryExp.Left is ConstantExpression ? binaryExp.Right : binaryExp.Left;
                    var otherDerivative = DifferentiateExpression(otherExpr, param);
                    return Expression.Multiply(constExpr, otherDerivative);
                }
                else
                {
                    var leftDerivative = DifferentiateExpression(binaryExp.Left, param);
                    var rightDerivative = DifferentiateExpression(binaryExp.Right, param);
                    return Expression.Add(
                        Expression.Multiply(binaryExp.Left, rightDerivative),
                        Expression.Multiply(binaryExp.Right, leftDerivative)
                    );
                }
            });

        _diffFuncs.Add(
            ExpressionType.Call,
            (expArg, param) =>
            { 
                var methodCallExp = (MethodCallExpression)expArg;
                if (!_supportedMethods.Contains(methodCallExp.Method.Name))
                {
                    throw new ArgumentException($"Method '{methodCallExp.Method.Name}' is not supported for differentiation");
                }
                if (methodCallExp.Method.Name == "Sin")
                {
                    return Expression.Multiply(
                        Expression.Call(null, typeof(Math).GetMethod("Cos", new[] { typeof(double) }), methodCallExp.Arguments[0]),
                        DifferentiateExpression(methodCallExp.Arguments[0], param)
                    );
                }
                if (methodCallExp.Method.Name == "Cos")
                {
                    var sinDerivative = Expression.Call(null, typeof(Math).GetMethod("Sin", new[] { typeof(double) }), methodCallExp.Arguments);

                    var cosDerivative = DifferentiateExpression(methodCallExp.Arguments[0], param);

                    var product = Expression.Multiply(sinDerivative, cosDerivative);
                    return Expression.Multiply(product, Expression.Constant(-1.0));
                }
                throw new ArgumentException($"Unsupported method call: {methodCallExp.Method.Name}");
            });

        _diffFuncs.Add(ExpressionType.Constant, (expArg, param) => Expression.Constant(0.0));
        _diffFuncs.Add(ExpressionType.Parameter, (expArg, param) => Expression.Constant(1.0));
    }

    private static Expression DifferentiateExpression(Expression expression, ParameterExpression parameter)
    {
        if (!_diffFuncs.ContainsKey(expression.NodeType))
        {
            throw new ArgumentException($"Unsupported expression type: {expression}");
        }

        return _diffFuncs[expression.NodeType](expression, parameter);
    }

    public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> funcExpr)
    {
        var parameter = funcExpr.Parameters[0];
        var expr = DifferentiateExpression(funcExpr.Body, parameter);
        var lambda = Expression.Lambda<Func<double, double>>(expr, parameter);
        return lambda;
    }
}