namespace Delegates.TreeTraversal;

public static class Traversal
{
    public static IEnumerable<Product> GetProducts(ProductCategory root)
    {
        return TraverseTree(
            root,
            category => category.Categories,
            category => category.Products
            );
    }

    public static IEnumerable<Job> GetEndJobs(Job root)
    {
        return TraverseTree(
            root,
            job => job.Subjobs,
            job => job.Subjobs.Count == 0
                ? new[] { job }
                : Enumerable.Empty<Job>()
            );
    }

    public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
    {
        return TraverseTree(
            root,
            node => new[] { node.Left, node.Right }.Where(child => child != null),
            node => (node.Left == null && node.Right == null)
                ? new[] { node.Value }
                : Enumerable.Empty<T>()
            );
    }

    public delegate IEnumerable<T> ChildrenSelector<T>(T node);
    public delegate IEnumerable<TOut> LeafSelector<TIn, TOut>(TIn node);

    public static IEnumerable<TOut> TraverseTree<TRoot, TOut>(
        TRoot root,
        ChildrenSelector<TRoot> childrenSelector,
        LeafSelector<TRoot, TOut> leafSelector)
    {
        foreach (var leaf in leafSelector(root))
        {
            yield return leaf;
        }

        foreach (var child in childrenSelector(root))
        {
            foreach (var leaf in TraverseTree(child, childrenSelector, leafSelector))
            {
                yield return leaf;
            }
        }
    }
}