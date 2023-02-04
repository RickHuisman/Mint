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
    }

    public void BeginScope() => _current.BeginScope();

    public void EndScope()
    {
        throw new NotImplementedException();
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
    private LocalsList Locals = new();
    private readonly CompilerInstance? _enclosing;
    private int scopeDepth { get; }

    public void BeginScope() => Locals.BeginScope();
    
    public void EndScope() => Locals.EndScope();
}

public class LocalsList
{
    private List<Local> _locals = new();
    public uint ScopeDepth { get; private set; }

    public void BeginScope()
    {
        ScopeDepth += 1;
    }

    public void EndScope()
    {
        // TODO: Close upvalues.
    }

    public void MarkInitialized()
    {
        // TODO:
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

public record Local(string Name, uint Depth, bool Initialized, uint Slot);