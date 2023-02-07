using System.Diagnostics;
using Mint.Compiler;
using ValueType = Mint.Compiler.ValueType;

namespace Mint.VM;

public class VM
{
    private Stack<Value> _stack = new();
    private IList<CallFrame> _frames = new List<CallFrame>();
    private Dictionary<string, Value> _globals = new();

    public void Run(FunctionProto functionProto)
    {
        _frames.Add(new CallFrame()
        {
            Ip = 0,
            StackStart = 0,
            FunctionProto = functionProto,
        });

        while (_frames.Count > 0)
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
                case Opcode.Pop:
                    Pop();
                    break;
                case Opcode.Return:
                    Return();
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

    private void Return()
    {
        var frame = _frames.Last();
        _frames.RemoveAt(_frames.Count - 1);
        return;
        
        // _stack.
        // _stack.Skip(frame.StackStart).Take(_stack.Count());

        // if let Some(frame) = self.frames_mut().pop() {
        //     let result = self.pop()?;
        //     self.stack_mut().truncate(*frame.stack_start());
        //     self.push(result);
        //     return Ok(());
        // }

        throw new Exception();
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

    private FunctionProto CurrentFunctionProto() => _frames.Last().FunctionProto;

    private void Push(Value value) => _stack.Push(value);

    private Value Pop() => _stack.Pop();

    public Value? Peek() => _stack.Peek();
}