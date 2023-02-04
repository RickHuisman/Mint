using System.Text;
using Mint.VM;

namespace Mint.Compiler;

public class FunctionProto
{
    public List<Value> Constants = new();
    private int _constantId;
    public List<Opcode> Opcodes = new();

    public Value GetConstant()
    {
        _constantId += 1;
        return Constants[_constantId - 1];
    }

    public void Write(Opcode op) => Opcodes.Add(op);

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
        builder.AppendLine("Instructions:");
        for (var i = 0; i < Opcodes.Count; i++)
        {
            builder.AppendLine($"{i} - {Opcodes[i]}");
        }

        return builder.ToString();
    }
}