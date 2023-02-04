using Mint.Compiler;

namespace Mint;

public static class Mint
{
    public static Value? Interpret(string source)
    {
        var compiler = new Compiler.Compiler();
        var functionProto = compiler.Compile(source);

        var vm = new VM.VM();
        vm.Run(functionProto);

        return vm.Peek();
    }
}