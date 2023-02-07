namespace Mint.Compiler;

public class CompilerInstance
{
    public Function Function = new(new FunctionProto());
    // private Local[] Locals = new Local[byte.MaxValue]; // 256 locals max. TODO: Use fixed size array for locals.
    public LocalsList Locals = new();
    public CompilerInstance? Enclosing = null;
    // private int scopeDepth { get; }

    public void BeginScope() => Locals.BeginScope();
    
    public void EndScope() => Locals.EndScope();
}

public class LocalsList
{
    public List<Local> Locals = new();
    public uint ScopeDepth { get; private set; }

    public void BeginScope()
    {
        ScopeDepth += 1;
    }

    public void EndScope()
    {
        // TODO: Close upvalues.
    }

    public void Insert(string name)
    {
        // TODO:
    }

    public void GetAtDepth()
    {
        // TODO:
    }
    
    public void GetAtCurrentDepth()
    {
        // TODO:
    }

    public Local? Get()
    {
        // TODO:
        return null;
    }
}

public record Local(string Name, uint Depth, uint Slot);