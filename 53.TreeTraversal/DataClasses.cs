namespace Delegates.TreeTraversal;

public class ProductCategory
{
	public List<Product> Products = new();
	public List<ProductCategory> Categories = new();
}

public class Product
{
	public string Name;
}

public class Job
{
	public string Name;
	public List<Job> Subjobs = new();
}

public class BinaryTree<T>
{
	public BinaryTree<T> Left;
	public BinaryTree<T> Right;
	public T Value;
}