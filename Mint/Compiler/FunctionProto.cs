using Mint.VM;

namespace Mint.Compiler;

public class FunctionProto
{
    public List<double> Constants = new();
    public List<Opcode> Opcodes = new();
}