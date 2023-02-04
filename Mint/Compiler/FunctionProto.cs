using System.Text; using Mint.VM;

namespace Mint.Compiler;

public class FunctionProto
{
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
        
        builder.AppendLine("== disarm <MAIN> ==");
        builder.AppendLine("Constants:");

        // Print constants.
        for (var i = 0; i < Constants.Count; i++)
        {
            builder.AppendLine($"{i} - {Constants[i]}");
        }
        
        // Print instructions.
        // TODO:
        // builder.AppendLine("Instructions:");
        // for (var i = 0; i < .Count; i++)
        // {
        //     builder.AppendLine($"{i} - {Opcodes[i]}");
        // }

        return builder.ToString();
    }
}