using Mint.Compiler;
using ValueType = Mint.Compiler.ValueType;
using DebugAssert = System.Diagnostics.Debug;

namespace Mint.VM;

public class VM
{
    private PeekableStack<Value> _stack = new();
    private IList<CallFrame> _frames = new List<CallFrame>();
    private Dictionary<string, Value> _globals = new();

    private VM()
    {
    }

    public static Value Interpret(string source, bool debug = false)
    {
        var compiler = new Compiler.Compiler();
        var function = compiler.Compile(source);
        function.FunctionProto.Name = "main";
        if (debug) Console.WriteLine(function.FunctionProto);
        var vm = new VM();
        return vm.Run(function);
    }

    private Value Run(Function function)
    {
        Push(new Value(function));
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
                case Opcode.Not:
                    Not();
                    break;
                case Opcode.Negate:
                    Negate();
                    break;
                case Opcode.Greater:
                    Greater();
                    break;
                case Opcode.Less:
                    Less();
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
                case Opcode.Jump:
                    Jump();
                    break;
                case Opcode.JumpIfFalse:
                    JumpIfFalse();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        PrintStack();
        return Peek();
    }

    private void ClosureInstruction()
    {
        var fun = ReadFunction();
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

        if (callee.ValueType == ValueType.Function)
        {
            Call(new Closure(callee.Function), arity);
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
        var b = Pop();
        var a = Pop();
        Push(a + b);
    }

    private void Subtract()
    {
        var b = Pop();
        var a = Pop();
        Push(a - b);
    }

    private void Multiply()
    {
        var b = Pop();
        var a = Pop();
        Push(a * b);
    }

    private void Divide()
    {
        var b = Pop();
        var a = Pop();
        Push(a / b);
    }

    private void Equal()
    {
        var b = Pop();
        var a = Pop();
        Push(a == b);
    }

    private void Not()
    {
        var a = Pop();
        Push(!a);
    }

    private void Negate()
    {
        var a = Pop();
        Push(-a);
    }

    private void Greater()
    {
        var b = Pop();
        var a = Pop();
        Push(a > b);
    }

    private void Less()
    {
        var b = Pop();
        var a = Pop();
        Push(a < b);
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

    private void Jump()
    {
        var offset = ReadShort();
        CurrentFrame().Ip += offset;
    }

    private void JumpIfFalse()
    {
        var offset = ReadShort();
        if (Peek().IsFalsey()) CurrentFrame().Ip += offset;
    }

    private void Return()
    {
        var frame = _frames.Last();
        var result = Pop();
        _stack.Truncate(frame.StackStart);
        Push(result);
        _frames.RemoveAt(_frames.Count - 1);
    }

    private Function ReadFunction()
    {
        var value = ReadConstant();
        DebugAssert.Assert(value.ValueType == ValueType.Function);
        return value.Function;
    }

    private string ReadString()
    {
        var name = ReadConstant();
        DebugAssert.Assert(name.ValueType == ValueType.String);
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

    private ushort ReadShort()
    {
        CurrentFrame().Ip += 2;
        return (ushort) ((CurrentFunctionProto().Code[CurrentFrame().Ip - 2] << 8) |
                         CurrentFunctionProto().Code[CurrentFrame().Ip - 1]);
    }

    private CallFrame CurrentFrame() => _frames.Last();

    private FunctionProto CurrentFunctionProto() => _frames.Last().Closure.Function.FunctionProto;

    private void Push(Value value) => _stack.Push(value);

    private Value Pop() => _stack.Pop();

    public Value? Peek() => _stack.Peek();

    public void PrintStack()
    {
        Console.WriteLine("== Stack ==");
        for (var i = _stack.Count() - 1; i >= 0; i--)
        {
            Console.WriteLine($"{i} - {_stack[i]}");
        }

        Console.WriteLine("===========\n");
    }
}