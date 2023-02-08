using System.Text;
using Mint.VM;

namespace Mint.Compiler;

public class FunctionProto
{
    public string Name { get; set; }
    public List<Value> Constants = new();
    public List<byte> Code = new();

    public byte AddConstant(Value constant)
    {
        Constants.Add(constant);
        return (byte) (Constants.Count - 1);
    }

    public void Write(Opcode op) => Code.Add((byte) op);

    public void Write(byte b) => Code.Add(b);

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.AppendLine($"== <{Name}> ==");
        for (var offset = 0; offset < Code.Count;)
        {
            offset = DisassembleInstruction(builder, offset);
        }

        return builder.ToString();
    }

    private int DisassembleInstruction(StringBuilder builder, int offset)
    {
        builder.Append($"{offset:X4} ");

        var instruction = (Opcode) Code[offset];
        return instruction switch
        {
            Opcode.LoadConstant => ConstantInstruction(builder, "load_constant", offset),
            Opcode.LoadNil => SimpleInstruction(builder, "load_nil", offset),
            Opcode.Add => SimpleInstruction(builder, "add", offset),
            Opcode.Subtract => SimpleInstruction(builder, "subtract", offset),
            Opcode.Multiply => SimpleInstruction(builder, "multiply", offset),
            Opcode.Divide => SimpleInstruction(builder, "divide", offset),
            Opcode.SetGlobal => ConstantInstruction(builder, "set_global", offset),
            Opcode.GetGlobal => ConstantInstruction(builder, "get_global", offset),
            Opcode.SetLocal => ByteInstruction(builder, "set_local", offset),
            Opcode.GetLocal => ByteInstruction(builder, "get_local", offset),
            Opcode.Equal => SimpleInstruction(builder, "equal", offset),
            Opcode.Print => SimpleInstruction(builder, "print", offset),
            Opcode.Pop => SimpleInstruction(builder, "pop", offset),
            Opcode.Return => SimpleInstruction(builder, "return", offset),
            Opcode.Closure => ConstantInstruction(builder, "closure", offset),
            Opcode.Call => ByteInstruction(builder, "call", offset),
            _ => throw new Exception($"Unknown opcode {instruction}")
        };
    }

    private static int SimpleInstruction(StringBuilder builder, string name, int offset)
    {
        builder.AppendLine(name);
        return offset + 1;
    }

    private int ConstantInstruction(StringBuilder builder, string name, int offset)
    {
        var constant = Code[offset + 1];
        builder.AppendLine($"{name,-16} '{Constants[constant]}'");
        return offset + 2;
    }

    private int ByteInstruction(StringBuilder builder, string name, int offset)
    {
        var slot = Code[offset + 1];

        var constant = Code[offset + 1];
        builder.AppendLine($"{name,-16} '{slot}'");
        return offset + 2;
    }
}