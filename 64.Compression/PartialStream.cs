namespace Streams.Compression;

public class PartialStream : Stream
{
	private readonly int partSize;
	private int index;
	private readonly byte[] content;

	public PartialStream(int partSize, byte[] content)
	{
		this.partSize = partSize;
		this.content = content;
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		count = Math.Min(partSize, count);
		for (var i = offset; i < offset + count; ++i)
			buffer[i] = content[index++ % content.Length];
		return count;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override bool CanRead => true;
	public override bool CanSeek => false;
	public override bool CanWrite => false;
	public override long Length => throw new NotSupportedException();

	public override long Position 
	{ 
		get => throw new NotSupportedException(); 
		set => throw new NotSupportedException(); 
	}
}