using Mint.Syntax;
using Mint.VM;

namespace Mint.Compiler;

public class Compiler
{
    private readonly FunctionProto _currentFunction = new();

    public FunctionProto Compile(string source)
    {
        var tokens = Lexer.Lex(source);
        var parser = new Parser();
        var ast = parser.ParseChunk(tokens);
        CompileChunk(ast);
        return _currentFunction;
    }

    private void CompileChunk(Chunk chunk)
    {
        foreach (var statement in chunk.Block.Statements)
        {
            statement.Compile(this);
        }
    }

    public void AddConstant(Value constant)
    {
        _currentFunction.Constants.Add(constant);
    }

    public void Emit(Opcode opcode) => _currentFunction.Write(opcode);
}