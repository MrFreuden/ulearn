using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T> where T : IComparable
{
    private TreeNode<T> _root;

    public void Add(T key)
    {
        if (_root == null)
        {
            _root = new TreeNode<T>(key);
        }
        else
        {
            TreeNode<T> current = _root;
            TreeNode<T> parent = null;

            while (current != null)
            {
                parent = current;
                parent.Size++;
                if (key.CompareTo(current.Value) >= 0)
                {
                    current = current.Right;
                }
                else
                {
                    current = current.Left;
                }
            }

            if (key.CompareTo(parent.Value) >= 0)
            {
                parent.Right = new TreeNode<T>(key);
            }
            else
            {
                parent.Left = new TreeNode<T>(key);
            }
        }
    }

    public bool Contains(T key)
    {
        TreeNode<T> current = _root;

        while (current != null)
        {
            if (key.CompareTo(current.Value) == 0)
            {
                return true;
            }
            if (key.CompareTo(current.Value) > 0)
            {
                current = current.Right;
            }
            else
            {
                current = current.Left;
            }
        }

        return false;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || (_root != null && index >= _root.Size))
            {
                throw new ArgumentOutOfRangeException();
            }
            return GetElementAt(_root, index);
        }
    }

    private T GetElementAt(TreeNode<T> node, int index)
    {
        if (node == null)
            throw new InvalidOperationException("Неверная операция");

        int leftSize = (node.Left != null) ? node.Left.Size : 0;

        if (index < leftSize)
        {
            return GetElementAt(node.Left, index);
        }
        else if (index == leftSize)
        {
            return node.Value;
        }
        else
        {
            return GetElementAt(node.Right, index - leftSize - 1);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return InOrderTraversal(_root).GetEnumerator();
    }

    private IEnumerable<T> InOrderTraversal(TreeNode<T> node)
    {
        if (node == null)
        {
            yield break;
        }

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

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class TreeNode<T>
{
    public int Size;
    public T Value;
    public TreeNode<T> Left, Right;
    public TreeNode(T value)
    {
        Value = value;
        Size = 1;
    }
}