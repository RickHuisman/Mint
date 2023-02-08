namespace Mint.VM;

public enum Opcode : byte
{
    Add,
    Subtract,
    Multiply,
    Divide,
    LoadNil,
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