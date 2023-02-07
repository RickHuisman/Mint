using Mint.Compiler;

namespace Mint.VM;

public class CallFrame
{
    public int Ip { get; set; }
    public int StackStart { get; set; }
    public FunctionProto FunctionProto { get; set; }
}