using Mint.Syntax;
using Mint.VM;

namespace Mint.Compiler;

public class Compiler
{
    private CompilerInstance _current = new();
    
    public Function Compile(string source)
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
        
        chunk.Block.ReturnStatement.Compile(this);
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

    public Function EndCompiler()
    {
        var funCopy = _current.Function;

        if (_current.Enclosing != null)
        {
            _current = _current.Enclosing;
        }

        return funCopy;
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

    public void EmitReturn()
    {
        Emit(Opcode.LoadNil);
        Emit(Opcode.Return);
    }
    
    public int EmitJump(Opcode opcode) {
        Emit(opcode);
        Emit(0xff);
        Emit(0xff);
        return CurrentFunctionProto().Code.Count - 2;
    }
    
    public void PatchJump(int offset)
    {
        // -2 to adjust for the bytecode for the jump offset itself.
        var jump = CurrentFunctionProto().Code.Count - offset - 2;

        CurrentFunctionProto().Code[offset] = (byte)((jump >> 8) & 0xff);
        CurrentFunctionProto().Code[offset + 1] = (byte)(jump & 0xff);
    }

    public void Emit(Opcode opcode) => CurrentFunctionProto().Write(opcode);
    
    public void Emit(byte b) => CurrentFunctionProto().Write(b);

    private FunctionProto CurrentFunctionProto()
    {
        return _current.Function.FunctionProto;
    }
}