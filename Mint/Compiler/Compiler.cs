using Mint.Syntax;
using Mint.VM;

namespace Mint.Compiler;

public class Compiler
{
    private FunctionProto _currentFunction = new();

    public FunctionProto Compile(string source)
    {
        var tokens = Lexer.Lex(source);
        var parser = new Parser();
        var ast = parser.ParseChunk(tokens);
        return CompileChunk(ast);
    }

    private FunctionProto CompileChunk(Chunk chunk)
    {
        foreach (var statement in chunk.Block.Statements)
        {
            statement.Compile(this);
        }

        return _currentFunction;
    }

    public void AddConstant(double constant)
    {
        _currentFunction.Constants.Add(constant);
    }

    public void Emit(Opcode opcode) => _currentFunction.Write(opcode);
}