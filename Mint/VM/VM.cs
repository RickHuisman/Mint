using System.Diagnostics;
using Mint.Compiler;
using ValueType = Mint.Compiler.ValueType;

namespace Mint.VM;

public class VM
{
    private Dictionary<string, Value> _globals = new();
    private Stack<Value> _stack = new();
    private FunctionProto _functionProto;
    
    public void Run(FunctionProto functionProto)
    {
        _functionProto = functionProto;
        
        foreach (var opcode in _functionProto.Opcodes)
        {
            switch (opcode)
            {
                case Opcode.Add:
                    var a = _stack.Pop();
                    var b = _stack.Pop();
                    _stack.Push(b + a);
                    break;
                case Opcode.LoadConstant:
                    _stack.Push(functionProto.GetConstant());
                    break;
                case Opcode.Equal:
                    var c = _stack.Pop();
                    var d = _stack.Pop();
                    _stack.Push(c == d);
                    break;
                case Opcode.NotEqual:
                    var e = _stack.Pop();
                    var f = _stack.Pop();
                    _stack.Push(e != f);
                    break;
                case Opcode.GetGlobal:
                    GetGlobal();
                    break;
                case Opcode.SetGlobal:
                    SetGlobal();
                    break;
            }
        }
    }

    private void GetGlobal()
    {
        var name = ReadConstant();
        Debug.Assert(name.ValueType == ValueType.String);
        var value = _globals[name.String];
        _stack.Push(value);
    }

    private void SetGlobal()
    {
        var name = ReadConstant();
        Debug.Assert(name.ValueType == ValueType.String);
        
        var value = Peek();
        _globals[name.String] = value;
    }

    private Value ReadConstant()
    {
        return _functionProto.GetConstant();
    }

    public Value Peek() => _stack.Peek();
}