using System.Diagnostics;
using Mint.Compiler;
using ValueType = Mint.Compiler.ValueType;

namespace Mint.VM;

public class VM
{
    private PeekableStack<Value> _stack = new();
    private IList<CallFrame> _frames = new List<CallFrame>();
    private Dictionary<string, Value> _globals = new();

    private VM()
    {
    }

    public static Value Interpret(string source)
    {
        var compiler = new Compiler.Compiler();
        var closure = compiler.Compile(source);
        closure.Function.FunctionProto.Name = "main";
        Console.WriteLine(closure.Function.FunctionProto);
        var vm = new VM();
        return vm.Run(closure);
    }

    private Value Run(Closure closure)
    {
        Push(new Value(closure));
        CallValue(0);

        while (_frames.Any())
        {
            var opcode = (Opcode) ReadByte();
            switch (opcode)
            {
                case Opcode.LoadNil:
                    LoadNil();
                    break;
                case Opcode.LoadConstant:
                    LoadConstant();
                    break;
                case Opcode.Add:
                    Add();
                    break;
                case Opcode.Subtract:
                    Subtract();
                    break;
                case Opcode.Multiply:
                    Multiply();
                    break;
                case Opcode.Divide:
                    Divide();
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
                case Opcode.Pop:
                    Pop();
                    break;
                case Opcode.Return:
                    Return();
                    break;
                case Opcode.Closure:
                    ClosureInstruction();
                    break;
                case Opcode.Call:
                    CallInstruction();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        return Peek();
    }

    private void ClosureInstruction()
    {
        var fun = ReadClosure();
        Push(new Value(fun));
    }

    private void CallInstruction()
    {
        var arity = ReadByte();
        CallValue(arity);
    }

    private void CallValue(int arity)
    {
        var frameStart = _stack.Count() - (arity + 1);
        var callee = _stack[frameStart];

        if (callee.ValueType == ValueType.Closure)
        {
            Call(callee.Closure, arity);
            return;
        }

        throw new Exception("Invalid callee");
    }

    private void Call(Closure closure, int arity)
    {
        var last = _stack.Count();
        var frameStart = last - (arity + 1);

        _frames.Add(new CallFrame
        {
            Ip = 0,
            StackStart = frameStart,
            Closure = closure,
        });
    }

    private void LoadNil()
    {
        Push(Value.NilValue());
    }

    private void LoadConstant()
    {
        Push(ReadConstant());
    }

    private void Add()
    {
        var a = Pop();
        var b = Pop();
        Push(b + a);
    }

    private void Subtract()
    {
        var a = Pop();
        var b = Pop();
        Push(b - a);
    }

    private void Multiply()
    {
        var a = Pop();
        var b = Pop();
        Push(b * a);
    }

    private void Divide()
    {
        var a = Pop();
        var b = Pop();
        Push(b / a);
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

    private void Return()
    {
        var frame = _frames.Last();
        var result = Pop();
        _stack.Truncate(frame.StackStart);
        Push(result);

        _frames.RemoveAt(_frames.Count - 1);
    }

    private Closure ReadClosure()
    {
        var value = ReadConstant();
        Debug.Assert(value.ValueType == ValueType.Closure);
        return value.Closure;
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
        return CurrentFunctionProto().Constants[index];
    }

    private byte ReadByte()
    {
        var b = CurrentFunctionProto().Code[CurrentFrame().Ip];
        CurrentFrame().Ip += 1;
        return b;
    }

    private CallFrame CurrentFrame() => _frames.Last();

    private FunctionProto CurrentFunctionProto() => _frames.Last().Closure.Function.FunctionProto;

    private void Push(Value value) => _stack.Push(value);

    private Value Pop() => _stack.Pop();

    public Value? Peek() => _stack.Peek();
}