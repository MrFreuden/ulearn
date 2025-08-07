using System.Collections;

namespace Generics.BinaryTrees;

public static class BinaryTree
{
    public static IEnumerable<int> Create(params int[] values)
    {
        var tree = new BinaryTree<int>();
        foreach (var val in values)
        {
            tree.Add(val);
        }
        return tree;
    }
}

public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
{
    public Node<T>? Root { get; private set; }
    public Node<T>? Left => Root?.Left;
    public Node<T>? Right => Root?.Right;
    public T? Value => Root != null ? Root.Value : default;


    public void Add(T value)
    {
        if (Root == null)
        {
            Root = new Node<T>(value);
        }
        else
        {
            Root.Add(value);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal(Root).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<T> InOrderTraversal(Node<T> node)
    {
        if (node == null) yield break;

        foreach (var leftValue in InOrderTraversal(node.Left))
        {
            yield return leftValue;
        }

        yield return node.Value;

        foreach (var rightValue in InOrderTraversal(node.Right))
        {
            yield return rightValue;
        }
    }
}

public class Node<T> where T : IComparable<T>
{
    public T? Value { get; private set; }
    public Node<T>? Left { get; private set; }
    public Node<T>? Right { get; private set; }

    public Node(T value)
    {
        Value = value;
    }
    
    public void Add(T newValue)
    {
        if (newValue.CompareTo(Value) <= 0)
        {
            if (Left == null)
            {
                Left = new Node<T>(newValue);
            }
            else
            {
                Left.Add(newValue);
            }
        }
        else
        {
            if (Right == null)
            {
                Right = new Node<T>(newValue);
            }
            else
            {
                Right.Add(newValue);
            }
        }
    }
}