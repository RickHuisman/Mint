namespace Mint.VM;

public enum Opcode
{
    Add,
    LoadConstant,
    Equal,
    NotEqual,
    GetGlobal,
    SetGlobal,
    Print, // TODO: Temporary opcode.
}