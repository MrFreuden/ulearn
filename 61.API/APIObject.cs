namespace Memory.API;

public class APIObject : IDisposable
{
    private readonly int _id;

    public APIObject(int id)
    {
        _id = id;
        MagicAPI.Allocate(_id);
    }

    private bool _isDisposed = false;

    ~APIObject()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool fromDisposeMethod)
    {
        if (!_isDisposed)
        {
            if (fromDisposeMethod)
            {   
            }
            
            MagicAPI.Free(_id);
            _isDisposed = true;
        }
    }
}