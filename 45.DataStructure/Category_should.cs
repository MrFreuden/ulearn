using NUnit.Framework;

// ReSharper disable IsExpressionAlwaysTrue

namespace Inheritance.DataStructure;

[TestFixture]
public class Category_should
{
	private Category A11 = new("A", MessageType.Incoming, MessageTopic.Subscribe);
	private Category A21 = new("A", MessageType.Outgoing, MessageTopic.Subscribe);
	private Category A12 = new("A", MessageType.Incoming, MessageTopic.Error);
	private Category B11 = new("B", MessageType.Incoming, MessageTopic.Subscribe);

	private Category[] Descending()
	{
		return new[] { A11, A12, A21, B11 };
	}

	private Category A11_copy = new("A", MessageType.Incoming, MessageTopic.Subscribe);

	[Test]
	public void ImplementEqualsCorrectly()
	{
		var a = Descending();
		for (var i = 0; i < a.Length; i++)
		for (var j = 0; j < a.Length; j++)
			Assert.AreEqual(i == j, a[i].Equals(a[j]), $"Error on {i} {j}");
		Assert.True(A11.Equals(A11_copy));
	}
        
	[Test]
	public void ImplementGetHashCodeCorrectly()
	{
		var a = Descending();
		for (var i = 0; i < a.Length; i++)
		for (var j = 0; j < a.Length; j++)
			if (i != j)
			{
				Assert.AreNotEqual(a[i].GetHashCode(), a[j].GetHashCode(), $"Error on {i} {j}");
				// Обычно от хеш-функции не требуется,
				// чтобы она возвращала разные значения на разных объектах.
				// Однако в этой задаче вам нужно сделать так,
				// чтобы на этом тесте разные объекты возвращали разные значений хеш-функций.
			}

		Assert.True(A11.Equals(A11_copy));
	}

	[Test]
	public void ImplementCompareToCorrectly()
	{
		var a = Descending();
		for (var i = 0; i < a.Length; i++)
		for (var j = 0; j < a.Length; j++)
			Assert.AreEqual(Math.Sign(i.CompareTo(j)), Math.Sign(a[i].CompareTo(a[j])), $"Error on {i} {j}");
		Assert.AreEqual(0, A11.CompareTo(A11_copy));
	}

	[Test]
	public void ImplementOperatorsCorrectly()
	{
		var a = Descending();
		for (var i = 0; i < a.Length; i++)
		for (var j = 0; j < a.Length; j++)
		{
			Assert.AreEqual(i <= j, a[i] <= a[j], $"Error on <=, {i} {j}");
			Assert.AreEqual(i >= j, a[i] >= a[j], $"Error on >=, {i} {j}");
			Assert.AreEqual(i < j, a[i] < a[j], $"Error on <, {i} {j}");
			Assert.AreEqual(i > j, a[i] > a[j], $"Error on >, {i} {j}");
			Assert.AreEqual(i == j, a[i] == a[j], $"Error on ==, {i} {j}");
			Assert.AreEqual(i != j, a[i] != a[j], $"Error on !=, {i} {j}");
		}
	}

	[Test]
	public void ImplementIComparableInterface()
	{
		Assert.True(A11 is IComparable);
	}

	[Test]
	public void ImplementToStringCorrectly()
	{
		Assert.AreEqual("A.Incoming.Subscribe", A11.ToString());
	}
}