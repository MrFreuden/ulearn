namespace Streams.Compression;

public class CustomCompressionStream : Stream
{
    private readonly bool _read;
    private readonly Stream _baseStream;
    private readonly Queue<byte> _decompressedBuffer = new();
    private bool _checkedOddLength = false;

    public CustomCompressionStream(Stream baseStream, bool read)
    {
        _read = read;
        _baseStream = baseStream;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (!CanRead)
            throw new InvalidOperationException("Stream is not readable");

        if (!CheckOddLength())
        {
            throw new InvalidOperationException("The length of the base stream is odd, which indicates invalid data for decompression.");
        }

        var bytesRead = 0;

        while (_decompressedBuffer.Count > 0 && bytesRead < count)
        {
            buffer[offset + bytesRead] = _decompressedBuffer.Dequeue();
            bytesRead++;
        }

        if (bytesRead == count)
            return bytesRead;

        while (bytesRead < count)
        {
            var val = _baseStream.ReadByte();
            if (val == -1) break;

            var runLength = _baseStream.ReadByte();
            if (runLength == -1) break;

            var bytesToWrite = Math.Min(runLength, count - bytesRead);

            for (int i = 0; i < bytesToWrite; i++)
            {
                buffer[offset + bytesRead] = (byte)val;
                bytesRead++;
            }

            for (int i = bytesToWrite; i < runLength; i++)
            {
                _decompressedBuffer.Enqueue((byte)val);
            }
        }

        return bytesRead;
    }

    private bool CheckOddLength()
    {
        if (!_checkedOddLength)
        {
            _checkedOddLength = true;
            if (_baseStream.CanSeek)
            {
                long remainingBytes = _baseStream.Length - _baseStream.Position;
                if (remainingBytes % 2 != 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!CanWrite)
            throw new InvalidOperationException("Stream is not writable");

        var i = offset;
        while (i < offset + count)
        {
            var val = buffer[i];
            var runLength = 1;

            while (i + runLength < offset + count && buffer[i + runLength] == val && runLength < 255)
            {
                runLength++;
            }

            _baseStream.WriteByte(val);
            _baseStream.WriteByte((byte)runLength);

            i += runLength;
        }
        _baseStream.Flush();
    }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override bool CanRead => _read;

    public override bool CanWrite => !_read;

    public override bool CanSeek => false;

    public override long Length => throw new NotSupportedException();

    public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }
}