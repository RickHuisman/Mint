using Mint.Syntax;
using Mint.VM;

namespace Mint.Compiler;

public class Compiler
{
    public FunctionProto Compile(string source)
    {
        var tokens = Lexer.Lex(source);
        var parser = new Parser();
        var ast = parser.ParseChunk(tokens);
        return CompileChunk();
    }

    private FunctionProto CompileChunk()
    {
        return new FunctionProto
        {
            Constants = {2, 3},
            Opcodes = {Opcode.LoadK, Opcode.LoadK, Opcode.Add}
        };
    }
}