using System.Text;

namespace Streams.Resources;

public class ResourceReaderStream : Stream
{
    private readonly string _key;
    private readonly Stream _baseStream;
    private byte[] _value;
    private int _position;


    public ResourceReaderStream(Stream stream, string key)
    {
        _key = key ?? throw new ArgumentNullException(nameof(key));
        _baseStream = new BufferedStream(stream ?? throw new ArgumentNullException(nameof(stream)),
            Constants.BufferSize);
    }

    private byte[] Value => _value ??= Initialize();

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_position >= Value.Length) return 0;

        int bytesToCopy = Math.Min(count, Value.Length - _position);
        Array.Copy(Value, _position, buffer, offset, bytesToCopy);
        _position += bytesToCopy;
        return bytesToCopy;
    }

    private byte[] Initialize()
    {
        var fieldReader = new FieldReader(_baseStream);
        while (true)
        {
            var keyBytes = fieldReader.ReadNextField();
            if (keyBytes == null)
                throw new EndOfStreamException($"Key '{_key}' not found before the end of the stream.");

            var keyString = Encoding.ASCII.GetString(keyBytes);
            var valueBytes = fieldReader.ReadNextField();

            if (valueBytes == null)
                throw new EndOfStreamException($"Value of key '{_key}' not found before the end of the stream.");

            if (keyString == _key)
                return valueBytes;
        }
    }
    
    public override void Flush()
    {
    }

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

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

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }
}

public class FieldReader
{
    private readonly Stream _stream;

    public FieldReader(Stream stream)
    {
        _stream = stream;
    }

    public byte[] ReadNextField()
    {
        var result = new List<byte>();
        bool lastWasZero = false;

        int nextByte;
        while ((nextByte = _stream.ReadByte()) != -1)
        {
            if (lastWasZero)
            {
                if (nextByte == 0)
                {
                    result.Add(0);
                    lastWasZero = false;
                }
                else if (nextByte == 1)
                {
                    return result.ToArray();
                }
                else
                {
                    result.Add(0);
                    result.Add((byte)nextByte);
                    lastWasZero = false;
                }
            }
            else if (nextByte == 0)
            {
                lastWasZero = true;
            }
            else
            {
                result.Add((byte)nextByte);
            }
        }

        return null;
    }
}