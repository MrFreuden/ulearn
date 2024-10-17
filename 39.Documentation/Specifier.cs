using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
    public string GetApiDescription()
    {
        var type = typeof(T);
        var attribute = type.GetCustomAttribute<ApiDescriptionAttribute>();
        if (attribute == null)
            return null;
        return attribute.Description;
    }

    public string[] GetApiMethodNames()
    {
        var type = typeof(T);
        var methods = type.GetMethods();
        if (methods == null) return null;
        var t = typeof(ApiMethodAttribute);
        return methods
            .Where(m => m.GetCustomAttributes(false)
                .Select(a => a.ToString() == t.ToString())
                .FirstOrDefault())
            .Select(m => m.Name)
            .ToArray();
    }

    public string GetApiMethodDescription(string methodName)
    {
        var type = typeof(T);
        var method = type.GetMethod(methodName);
        if (method == null) return null;
        return method.CustomAttributes
            .FirstOrDefault(x => x.AttributeType == typeof(ApiDescriptionAttribute))?
            .ConstructorArguments
            .First().Value?.ToString();
    }

    public string[] GetApiMethodParamNames(string methodName)
    {
        var type = typeof(T);
        var method = type.GetMethod(methodName);
        if (method == null) return null;

        return method.GetParameters()
            .Select(x => x.Name.ToString())
            .ToArray();
    }

    public string GetApiMethodParamDescription(string methodName, string paramName)
    {
        var type = typeof(T);
        var method = type.GetMethod(methodName);
        if (method == null) return null;
        var param = method.GetParameters().Where(p => p.Name == paramName).FirstOrDefault();
        if (param == null) return null;

        return param.CustomAttributes
            .FirstOrDefault(x => x.AttributeType == typeof(ApiDescriptionAttribute))?
            .ConstructorArguments
            .First().Value?.ToString();
    }

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
    {
        var type = typeof(T);
        var method = type.GetMethod(methodName);
        if (method == null) return GetDefaultMethodParamFullDescription(paramName);

        var param = method.GetParameters().FirstOrDefault(p => p.Name == paramName);
        if (param == null) return GetDefaultMethodParamFullDescription(paramName);

        var paramDescription = new CommonDescription(
            paramName,
            param.GetCustomAttribute<ApiDescriptionAttribute>()?.Description
        );

        var required = param.GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false;
        var minValue = param.GetCustomAttribute<ApiIntValidationAttribute>()?.MinValue;
        var maxValue = param.GetCustomAttribute<ApiIntValidationAttribute>()?.MaxValue;

        return new ApiParamDescription
        {
            ParamDescription = paramDescription,
            Required = required,
            MinValue = minValue,
            MaxValue = maxValue
        };
    }

    private ApiParamDescription GetDefaultMethodParamFullDescription(string paramName)
    {
        return new ApiParamDescription
        {
            ParamDescription = new CommonDescription(paramName),
            Required = false,
            MinValue = null,
            MaxValue = null
        };
    }

    public ApiMethodDescription GetApiMethodFullDescription(string methodName)
    {
        var type = typeof(T);
        var method = type.GetMethod(methodName);
        if (method == null) return null;

        var apiMethodAttribute = method.GetCustomAttribute<ApiMethodAttribute>();
        if (apiMethodAttribute == null) return null;

        var methodDescription = new CommonDescription(
            methodName,
            method.GetCustomAttribute<ApiDescriptionAttribute>()?.Description
        );

        var paramDescriptions = method.GetParameters().Select(param =>
        {
            var paramDescription = new CommonDescription(
                param.Name,
                param.GetCustomAttribute<ApiDescriptionAttribute>()?.Description
            );

            var required = param.GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false;
            var minValue = param.GetCustomAttribute<ApiIntValidationAttribute>()?.MinValue;
            var maxValue = param.GetCustomAttribute<ApiIntValidationAttribute>()?.MaxValue;

            return new ApiParamDescription
            {
                ParamDescription = paramDescription,
                Required = required,
                MinValue = minValue,
                MaxValue = maxValue
            };
        }).ToArray();

        ApiParamDescription returnDescription = null;
        if (method.ReturnType != typeof(void))
        {
            returnDescription = new ApiParamDescription
            {
                ParamDescription = new CommonDescription(
                    null,
                    method.ReturnParameter?.GetCustomAttribute<ApiDescriptionAttribute>()?.Description
                ),
                Required = method.ReturnParameter?.GetCustomAttribute<ApiRequiredAttribute>()?.Required ?? false,
                MinValue = method.ReturnParameter?.GetCustomAttribute<ApiIntValidationAttribute>()?.MinValue,
                MaxValue = method.ReturnParameter?.GetCustomAttribute<ApiIntValidationAttribute>()?.MaxValue
            };
        }

        return new ApiMethodDescription
        {
            MethodDescription = methodDescription,
            ParamDescriptions = paramDescriptions,
            ReturnDescription = returnDescription
        };
    }
}