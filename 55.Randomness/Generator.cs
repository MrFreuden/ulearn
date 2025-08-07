using System.Reflection;

namespace Reflection.Randomness;

public class Generator<T>
{
    private readonly record struct DistributionInfo(
        object Instance,
        MethodInfo GenerateMethod,
        PropertyInfo Property);

    private readonly List<DistributionInfo> _distributionInfos;

    public Generator()
    {
        _distributionInfos = InitializeDistributions();
    }

    public T Generate(Random rnd)
    {
        var instance = Activator.CreateInstance<T>();

        foreach (var info in _distributionInfos)
        {
            var generateValue = info.GenerateMethod.Invoke(info.Instance, new object[] { rnd });
            info.Property.SetValue(instance, generateValue);
        }
        return instance;
    }

    private List<DistributionInfo> InitializeDistributions()
    {
        return GetPropertiesWithDistributions()
            .Select(CreateDistributionInfo)
            .Where(info => info.HasValue)
            .Select(info => info.Value)
            .ToList();
    }

    private IEnumerable<PropertyInfo> GetPropertiesWithDistributions()
    {
        return typeof(T)
            .GetProperties()
            .Where(HasDistributionAttribute);
    }

    private bool HasDistributionAttribute(PropertyInfo property)
    {
        return property.GetCustomAttributes(typeof(FromDistribution), false).Length > 0;
    }

    private DistributionInfo? CreateDistributionInfo(PropertyInfo property)
    {
        var attribute = GetDistributionAttribute(property);
        if (attribute?.DistributionType == null) return null;

        ValidateDistributionType(attribute.DistributionType);

        var constructor = GetConstructor(attribute)
            ?? throw new ArgumentException(attribute.DistributionType.ToString() +
                                                        "does not have a suitable constructor");

        var instance = constructor.Invoke(attribute.Values.Cast<object>().ToArray());
        var generateMethod = GetGenerateMethod(attribute.DistributionType);

        return new DistributionInfo(instance, generateMethod, property);
    }

    private FromDistribution? GetDistributionAttribute(PropertyInfo property)
    {
        return (FromDistribution)property
                .GetCustomAttributes(typeof(FromDistribution), false)
                .FirstOrDefault();
    }

    private void ValidateDistributionType(Type type)
    {
        if (!typeof(IContinuousDistribution).IsAssignableFrom(type))
            throw new ArgumentException(type.ToString());
    }

    private ConstructorInfo? GetConstructor(FromDistribution attribute)
    {
        return attribute.DistributionType
                .GetConstructors()
                .FirstOrDefault(c => c.GetParameters().Length == attribute.Values.Count);
    }

    private MethodInfo GetGenerateMethod(Type type)
    {
        return type.GetMethod("Generate", new[] { typeof(Random) })
            ?? throw new MissingMethodException(
                $"{type.FullName} does not have a method Generate(Random)");
    }
}