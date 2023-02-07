using Mint.VM;
using NUnit.Framework;

namespace Silver.Test;

public class PeekableStackTest
{
    [Test]
    public void Stack_Test()
    {
        var stack = new PeekableStack<int>();
        
        stack.Push(2);
        stack.Push(3);
        
        Assert.AreEqual(3, stack.Peek());
        
        stack.Push(4);
        stack.Push(5);
        stack.Pop();
        var pop = stack.Pop();
        
        Assert.AreEqual(4, pop);
        
        stack.Push(6);
        
        Assert.AreEqual(6, stack.Peek());

        stack[0] = 10;
        Assert.AreEqual(stack[0], 10);
    }

    [Test]
    public void Stack_Truncate_Test()
    {
        var stack = new PeekableStack<int>();
        
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        stack.Push(4);
        stack.Push(5);
        stack.Push(6);

        Assert.AreEqual(stack.Count(), 6);
        
        stack.Truncate(4);
        
        Assert.AreEqual(stack.Count(), 2);
        Assert.AreEqual(stack.Peek(), 2);
    }
}