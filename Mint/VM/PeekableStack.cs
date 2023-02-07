namespace Mint.VM;

public class PeekableStack<T>
{
    private readonly List<T> _inner = new(); // TODO: Max size.

    public T this[int index]
    {
        get => _inner[index];
        set => _inner[index] = value;
    }

    public void Push(T value) => _inner.Add(value);

    public T Pop()
    {
        var ret = _inner.Last();
        _inner.RemoveAt(_inner.Count - 1);
        return ret;
    }

    public T Peek()
    {
        if (_inner.Count == 0) throw new Exception();
        return _inner.Last();
    }

    public int Count() => _inner.Count;

    public void Truncate(int length)
    {
        _inner.RemoveRange(_inner.Count - length, length);
    }
}