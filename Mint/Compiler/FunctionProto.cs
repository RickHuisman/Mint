using Mint.VM;

namespace Mint.Compiler;

public class FunctionProto
{
    public List<double> Constants = new();
    private int _constantId = 0;
    public List<Opcode> Opcodes = new();

    public double GetConstant()
    {
        _constantId += 1;
        return Constants[_constantId - 1];
    }

    public void Write(Opcode op) => Opcodes.Add(op);
}