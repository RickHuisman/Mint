using Mint.Syntax;
using Mint.VM;

namespace Mint.Compiler;

public class Compiler
{
    private CompilerInstance _current = new();
    
    public Closure Compile(string source)
    {
        var tokens = Lexer.Lex(source);
        var parser = new Parser();
        var ast = parser.ParseChunk(tokens);
        CompileChunk(ast);
        return EndCompiler();
    }

    private void CompileChunk(Chunk chunk)
    {
        foreach (var statement in chunk.Block.Statements)
        {
            statement.Compile(this);
        }
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

    public void SetInstance(CompilerInstance instance)
    {
        // TODO: Copy works?
        var currentCopy = _current;
        _current = instance;
        _current.Enclosing = currentCopy;
    }

    public Closure EndCompiler()
    {
        EmitReturn();
        var funCopy = _current.Function;

        Console.WriteLine(CurrentFunctionProto());

        if (_current.Enclosing != null)
        {
            _current = _current.Enclosing;
        }

        return new Closure(funCopy);
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
        return CurrentFunctionProto().AddConstant(constant);
    }

    private void EmitReturn()
    {
        // TODO: Emit nil.
        Emit(Opcode.Return);
    }

    public void Emit(Opcode opcode) => CurrentFunctionProto().Write(opcode);
    
    public void Emit(byte b) => CurrentFunctionProto().Write(b);

    private FunctionProto CurrentFunctionProto()
    {
        return _current.Function.FunctionProto;
    }
}