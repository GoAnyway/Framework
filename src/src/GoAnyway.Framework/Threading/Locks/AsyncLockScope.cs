namespace GoAnyway.Framework.Threading.Locks;

public readonly struct AsyncLockScope : IDisposable
{
    private readonly AsyncLock _asyncLock;

    public AsyncLockScope(AsyncLock asyncLock)
    {
        _asyncLock = asyncLock;
    }

    public void Dispose()
    {
        _asyncLock.Release();
    }
}