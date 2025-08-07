namespace Inheritance.DataStructure;

public class Category : IComparable
{
    private readonly string _name;
    private readonly MessageType _messageType;
    private readonly MessageTopic _messageTopic;

    public Category(string name, MessageType messageType, MessageTopic messageTopic)
    {
        _name = name;
        _messageType = messageType;
        _messageTopic = messageTopic;
    }

    public string Name => _name;
    public MessageType MessageType => _messageType;
    public MessageTopic MessageTopic => _messageTopic;

    public override string ToString()
    {
        return $"{_name}.{_messageType}.{_messageTopic}";
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;

            hash = hash * 23 + (_name?.GetHashCode() ?? 0);
            hash = hash * 23 + _messageType.GetHashCode();
            hash = hash * 23 + _messageTopic.GetHashCode();

            return hash;
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj is not Category other) return false;

        return _name == other?._name 
            && _messageType == other._messageType 
            && _messageTopic == other._messageTopic;
    }

    public int CompareTo(object? obj)
    {
        if (obj == null) return 1;
        if (obj is not Category other) throw new ArgumentException("Object is not a Category");
        if (Name is null || other.Name is null) return 1;

        var nameComparison = _name.CompareTo(other._name);
        if (nameComparison != 0) return nameComparison;

        var typeComparison = MessageType.CompareTo(other.MessageType);
        if (typeComparison != 0) return typeComparison;

        return MessageTopic.CompareTo(other.MessageTopic);
    }

    public static bool operator ==(Category c1, Category c2) => c1.Equals(c2);
    public static bool operator !=(Category c1, Category c2) => !c1.Equals(c2);
    public static bool operator >(Category c1, Category c2) => c1.CompareTo(c2) > 0;
    public static bool operator <(Category c1, Category c2) => c1.CompareTo(c2) < 0;
    public static bool operator >=(Category c1, Category c2) => c1.CompareTo(c2) >= 0;
    public static bool operator <=(Category c1, Category c2) => c1.CompareTo(c2) <= 0;
}