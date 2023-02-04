using System.Diagnostics;
using Mint.Compiler;
using ValueType = Mint.Compiler.ValueType;

namespace Mint.VM;

public class VM
{
    private int _ip;
    private Stack<Value> _stack = new();
    private Dictionary<string, Value> _globals = new();
    private FunctionProto _functionProto;

    public void Run(FunctionProto functionProto)
    {
        _functionProto = functionProto;

        while (_ip < _functionProto.Code.Count)
        {
            var opcode = (Opcode) ReadByte();
            switch (opcode)
            {
                case Opcode.Add:
                    Add();
                    break;
                case Opcode.LoadConstant:
                    LoadConstant();
                    break;
                case Opcode.Equal:
                    Equal();
                    break;
                case Opcode.NotEqual:
                    NotEqual();
                    break;
                case Opcode.SetLocal:
                    SetLocal();
                    break;
                case Opcode.GetLocal:
                    GetLocal();
                    break;
                case Opcode.GetGlobal:
                    GetGlobal();
                    break;
                case Opcode.SetGlobal:
                    SetGlobal();
                    break;
                case Opcode.Print:
                    Print();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void Add()
    {
        var a = Pop();
        var b = Pop();
        Push(b + a);
    }

    private void LoadConstant()
    {
        Push(ReadConstant());
    }

    private void Equal()
    {
        var a = Pop();
        var b = Pop();
        Push(a == b);
    }

    private void NotEqual()
    {
        var a = Pop();
        var b = Pop();
        Push(a != b);
    }

    private void SetLocal()
    {
        var slot = ReadByte();
        _stack[slot] = Peek();
    }

    private void GetLocal()
    {
        var slot = ReadByte();
        Push(_stack[slot]);
    }

    private void GetGlobal()
    {
        var name = ReadString();
        var value = _globals[name];
        Push(value);
    }

    private void SetGlobal()
    {
        var name = ReadString();
        var value = Peek();
        _globals[name] = value;
    }

    private void Print()
    {
        var value = Pop();
        Console.WriteLine(value);
    }

    private string ReadString()
    {
        var name = ReadConstant();
        Debug.Assert(name.ValueType == ValueType.String);
        return name.String;
    }

    private Value ReadConstant()
    {
        var index = ReadByte();
        return _functionProto.Constants[index];
    }

    private byte ReadByte()
    {
        var b = _functionProto.Code[_ip];
        _ip += 1;
        return b;
    }

    private void Push(Value value) => _stack.Push(value);
    
    private Value Pop() => _stack.Pop();

    public Value? Peek() => _stack.Peek();
}