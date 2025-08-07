namespace FluentApi.Graph;

public class GraphNode
{
    public readonly Dictionary<string, string> Attributes = new();
    public string Name { get; }

    public GraphNode(string name)
    {
        Name = name;
    }
}