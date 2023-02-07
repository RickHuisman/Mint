using System.Collections;

namespace Mint.VM;

public class Stack<T> : IEnumerable<T>
{
    private readonly T[] _inner = new T[64]; // TODO: 64?
    private int _stackTop;

    public T this[int index]
    {
        get => _inner[index];
        set => _inner[index] = value;
    }

    public void Push(T value) => _inner[_stackTop++] = value;

    public T Pop() => _inner[--_stackTop];

    public T Peek()
    {
        if (_stackTop == 0) throw new Exception();
        return _inner[_stackTop - 1];
    }

    public T First() => _inner[_stackTop - 1];

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var o in _inner)
        {
            if (o == null)
            {
                break;
            }

            yield return o;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}