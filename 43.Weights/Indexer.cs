namespace Incapsulation.Weights;

public class Indexer
{
    private readonly double[] _weights;
	public readonly int StartIndex;
	public readonly int Length;

	public Indexer(double[] range, int start, int length)
	{
		if (start < 0 || length < 0 || length > range.Length - start) throw new ArgumentOutOfRangeException();
		_weights = range;
		StartIndex = start;
		Length = length;
	}

	public double this[int index]
	{
		get 
		{
            if (index < 0 || index >= Length)
            {
				throw new IndexOutOfRangeException();
            }
            return _weights[index + StartIndex]; 
		}
		set 
		{
			if (index < 0 || index >= Length)
			{
				throw new IndexOutOfRangeException();
            }
			_weights[index + StartIndex] = value; 
		}
	}
}