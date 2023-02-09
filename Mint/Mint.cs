using Mint.Compiler;

namespace Mint;

public static class Mint
{
    public static Value Interpret(string source) => VM.VM.Interpret(source);
}