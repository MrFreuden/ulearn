using NUnit.Framework;

namespace Delegates.TreeTraversal;

[TestFixture]
public class Traversal_should
{
	public void Test<T, TIntermediate>(IEnumerable<T> actual,
		Func<T, TIntermediate> selector,
		params TIntermediate[] expected)
	{
		var a = actual.Select(selector).ToList();
		CollectionAssert.AreEquivalent(expected, a);
	}

	[Test]
	public void GetBinaryTreeValues()
	{
		var data = new BinaryTree<int>
		{
			Value = 0,
			Left = new BinaryTree<int>
			{
				Value = 1,
				Left = new BinaryTree<int>
				{
					Value = 3
				},
				Right = new BinaryTree<int>
				{
					Value = 5,
					Left = new BinaryTree<int>
					{
						Value = 7
					},
					Right = new BinaryTree<int>
					{
						Value = 9
					}
				}
			},
			Right = new BinaryTree<int>
			{
				Value = 11
			}
		};

		Test(Traversal.GetBinaryTreeValues(data), z => z, 3, 7, 9, 11);
	}

	[Test]
	public void GetEndJobs()
	{
		var data = new Job
		{
			Name = "4",
			Subjobs = new List<Job>
			{
				new()
				{
					Name="3"
				},
				new()
				{
					Name="A",
					Subjobs=new List<Job>
					{
						new()
						{
							Name="1"
						},
						new()
						{
							Name="2"
						}
					}

				}
			}
		};
		Test(Traversal.GetEndJobs(data), z => z.Name, "1", "2", "3");

	}

	[Test]
	public void GetProducts()
	{
		var data = new ProductCategory
		{
			Categories = new List<ProductCategory>
			{
				new()
				{
					Products=new List<Product>
					{
						new()
						{
							Name="X"
						},
						new()
						{
							Name="Y"
						}
					}
				},
				new()
				{
					Products=new List<Product>
					{
						new()
						{
							Name="1"
						}
					}
				}
			},
			Products = new List<Product>
			{
				new()
				{
					Name="A"
				}
			}
		};

		Test(Traversal.GetProducts(data), z => z.Name, "X", "Y", "1", "A");
	}
}