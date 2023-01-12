using Mint.Compiler;

namespace Mint.VM;

public class VM
{
    private Stack<Value> _stack = new();
    
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
                    _stack.Push(functionProto.GetConstant());
                    break;
                case Opcode.Equal:
                    var c = _stack.Pop();
                    var d = _stack.Pop();
                    _stack.Push(c == d);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public Value Peek() => _stack.Peek();
}