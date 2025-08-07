using System.Reflection;
using System.Text;

namespace Ddd.Taxi.Infrastructure;

/// <summary>
/// Базовый класс для всех Value типов.
/// </summary>
public class ValueType<T>
{
    private readonly PropertyInfo[] _properties;

    public ValueType()
    {
        _properties = GetProps();
    }

    private PropertyInfo[] GetProps()
    {
        return typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public bool Equals(T other)
    {
        if (other == null)
            return false;

        for (int i = 0; i < _properties.Length; i++)
        {
            var val1 = _properties[i].GetValue(this);
            var val2 = _properties[i].GetValue(other);
            if (val1 == null && val2 == null) continue;
            if (val1 == null || !val1.Equals(val2))
            {
                return false;
            }
        }

        return true;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;

        return Equals((T)obj);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var property in _properties)
        {
            var value = property.GetValue(this);
            hash = hash * 31 + (value?.GetHashCode() ?? 0);
        }
        return hash;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        var sortedProperties = _properties
            .Select(p => (p.Name, p.GetValue(this)))
            .OrderBy(x => x.Name);

        foreach (var prop in sortedProperties)
            sb.Append(new string($"{prop.Name}: {prop.Item2}; "));

        if (sb.Length > 2) sb.Length -= 2;

        return $"{typeof(T).Name}({sb})";
    }
}
