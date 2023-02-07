using Mint.Syntax;
using Mint.VM;

namespace Mint.Compiler;

public class Compiler
{
    private CompilerInstance _current = new();
    
    public FunctionProto Compile(string source)
    {
        var tokens = Lexer.Lex(source);
        var parser = new Parser();
        var ast = parser.ParseChunk(tokens);
        CompileChunk(ast);
        return _current.Function;
    }

    private void CompileChunk(Chunk chunk)
    {
        foreach (var statement in chunk.Block.Statements)
        {
            statement.Compile(this);
        }
        Emit(Opcode.Return);
    }
    
    public void DeclareVariable(string name)
    {
        if (_current.Locals.Locals.Any(l => name == l.Name))
        {
            throw new Exception();
        }

        // Add local.
        if (_current.Locals.Locals.Count == byte.MaxValue)
        {
            throw new Exception();
            // TODO: Throw error if too many locals.
        }

        var local = new Local(name, 0, (uint) _current.Locals.Locals.Count);
        _current.Locals.Locals.Add(local);
    }

    public int ResolveLocal(string name)
    {
        foreach (var local in _current.Locals.Locals.Where(local => name == local.Name))
        {
            return (int) local.Slot;
        }

        return -1;
    }

    public void BeginScope() => _current.BeginScope();

    public void EndScope()
    {
        for (var i = 0; i < _current.Locals.Locals.Count; i++)
        {
            Emit(Opcode.Pop);
        }
    }

    public byte AddConstant(Value constant)
    {
        return _current.Function.AddConstant(constant);
    }

    public void Emit(Opcode opcode) => _current.Function.Write(opcode);
    
    public void Emit(byte b) => _current.Function.Write(b);
}

public class CompilerInstance
{
    public readonly FunctionProto Function = new();
    // private Local[] Locals = new Local[byte.MaxValue]; // 256 locals max. TODO: Use fixed size array for locals.
    public LocalsList Locals = new();
    private readonly CompilerInstance? _enclosing;
    private int scopeDepth { get; }

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