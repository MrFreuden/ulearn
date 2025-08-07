using System.Globalization;
using System.Reflection.Emit;

namespace FluentApi.Graph;

public class DotGraphBuilder :
    IConfigureGraph, IConfigureGraphEdge, IConfigureGraphNode
{
    private readonly Graph _graph;
    private GraphEdge _lastAddedEdge;
    private GraphNode _lastAddedNode;

    private DotGraphBuilder(string graphName, bool isDirected)
    {
        _graph = new Graph(graphName, isDirected, false);
    }

    public static IConfigureGraph DirectedGraph(string graphName)
    {
        return new DotGraphBuilder(graphName, true);
    }

    public static IConfigureGraph UndirectedGraph(string graphName)
    {
        return new DotGraphBuilder(graphName, false);
    }

    public IConfigureGraphEdge AddEdge(string sourceNode, string destinationNode)
    {
        _lastAddedEdge = _graph.AddEdge(sourceNode, destinationNode);
        return this;
    }

    public IConfigureGraphNode AddNode(string name)
    {
        _lastAddedNode = _graph.AddNode(name);
        return this;
    }

    public string Build()
    {
        return _graph.ToDotFormat();
    }

    public IConfigureGraph With(Action<IConfigureEdge> value)
    {
        if (_lastAddedEdge != null)
        {
            var configurator = new EdgeAttributeConfigurator();
            value.Invoke(configurator);

            foreach (var attribute in configurator.Attributes)
            {
                _lastAddedEdge.Attributes[attribute.Key] = attribute.Value;
            }
        }
        return this;
    }

    public IConfigureGraph With(Action<IConfigureNode> value)
    {
        if (_lastAddedNode != null)
        {
            var configurator = new NodeAttributeConfigurator();
            value.Invoke(configurator);

            foreach (var attribute in configurator.Attributes)
            {
                _lastAddedNode.Attributes[attribute.Key] = attribute.Value;
            }
        }
        return this;
    }
}

public abstract class AttributeConfigurator<TSelf, TInterface> : IConfigureGraphElements<TInterface>
    where TSelf : AttributeConfigurator<TSelf, TInterface>, TInterface 
    where TInterface : IConfigureGraphElements<TInterface>
{
    protected readonly Dictionary<string, string> _attributes = new();

    public IReadOnlyDictionary<string, string> Attributes => _attributes;

    public TInterface Color(string color)
    {
        _attributes["color"] = color.ToLower();
        return (TSelf)this;
    }

    public TInterface FontSize(int size)
    {
        _attributes["fontsize"] = size.ToString();
        return (TSelf)this;
    }

    public TInterface Label(string label)
    {
        _attributes["label"] = label.ToLower();
        return (TSelf)this;
    }
}

public class NodeAttributeConfigurator 
    : AttributeConfigurator<NodeAttributeConfigurator, IConfigureNode>, IConfigureNode
{
    public IConfigureNode Shape(NodeShape shape)
    {
        _attributes["shape"] = shape.ToString().ToLower();
        return this;
    }
}

public class EdgeAttributeConfigurator 
    : AttributeConfigurator<EdgeAttributeConfigurator, IConfigureEdge>, IConfigureEdge
{
    public IConfigureEdge Weight(double weight)
    {
        _attributes["weight"] = weight.ToString(CultureInfo.InvariantCulture);
        return this;
    }
}

public interface IConfigureGraph
{
    IConfigureGraphEdge AddEdge(string sourceNode, string destinationNode);
    IConfigureGraphNode AddNode(string name);
    string Build();
}

public interface IConfigureGraphEdge : IConfigureGraph
{
    IConfigureGraph With(Action<IConfigureEdge> value);
}

public interface IConfigureGraphNode : IConfigureGraph
{
    IConfigureGraph With(Action<IConfigureNode> value);
}

public interface IConfigureGraphElements<T> where T : IConfigureGraphElements<T>
{
    T Color(string color);
    T FontSize(int size);
    T Label(string label);
}

public interface IConfigureEdge : IConfigureGraphElements<IConfigureEdge>
{
    IConfigureEdge Weight(double weight);
}

public interface IConfigureNode : IConfigureGraphElements<IConfigureNode>
{
    IConfigureNode Shape(NodeShape shape);
}

public enum NodeShape
{
    Box,
    Ellipse
}