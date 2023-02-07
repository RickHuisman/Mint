namespace Mint.VM;

public enum Opcode : byte
{
    Add,
    LoadConstant,
    Equal,
    NotEqual,
    SetLocal,
    GetLocal,
    GetGlobal,
    SetGlobal,
    Print, // TODO: Temporary opcode.
    Closure,
    Call,
    Return,
    Pop,
}