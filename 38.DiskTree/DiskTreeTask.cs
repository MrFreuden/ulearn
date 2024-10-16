using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree;

public class DiskTreeTask
{
    public static List<string> Solve(List<string> list)
    {
        var root = new TreeNode<string>("root");
        
        foreach (var path in list)
        {
            TreeNode<string> currentNode = root;
            foreach (var name in path.Split('\\'))
            {
                var existingNode = currentNode.Children.FirstOrDefault(x => x.Data == name);
                if (existingNode == null)
                {
                    currentNode = currentNode.AddChild(name);
                }
                else
                {
                    currentNode = existingNode;
                }
            }
        }

        return Perform(root);
    }

    private static List<string> Perform(TreeNode<string> root)
    {
        var result = new List<string>();
        foreach (var item in root)
        {
            string prefix = item.Level > 1 ? new string(' ', item.Level - 1) : "";
            result.Add(new string(prefix + item.Data));
        }
        return result.Skip(1).ToList();
    }
}

public class TreeNode<T> : IEnumerable<TreeNode<T>> where T : IComparable<T>
{
    public T Data { get; set; }
    public TreeNode<T> Parent { get; set; }
    public ICollection<TreeNode<T>> Children { get; set; }

    public bool IsRoot => Parent == null;
    public int Level => IsRoot ? 0 : Parent.Level + 1;

    public TreeNode(T data)
    {
        Data = data;
        Children = new SortedSet<TreeNode<T>>(Comparer<TreeNode<T>>.Create((x, y) =>
                StringComparer.Ordinal.Compare(x.Data.ToString(), y.Data.ToString())));
    }

    public TreeNode<T> AddChild(T child)
    {
        var childNode = new TreeNode<T>(child) { Parent = this };
        Children.Add(childNode);
        return childNode;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<TreeNode<T>> GetEnumerator()
    {
        yield return this;
        foreach (var directChild in Children)
        {
            foreach (var anyChild in directChild)
                yield return anyChild;
        }
    }
}