using Mint.Compiler;

namespace Mint.VM;

public class VM
{
    private Stack<double> _stack = new();
    
    public void Run(FunctionProto functionProto)
    {
        foreach (var opcode in functionProto.Opcodes)
        {
            switch (opcode)
            {
                case Opcode.Add:
                    var a = _stack.Pop();
                    var b = _stack.Pop();
                    _stack.Push(b + a);
                    break;
                case Opcode.LoadK:
                    _stack.Push(2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public double Peek() => _stack.Peek();
}