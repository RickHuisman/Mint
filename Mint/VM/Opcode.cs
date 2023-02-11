namespace Mint.VM;

public enum Opcode : byte
{
    LoadNil,
    LoadConstant,
    Add,
    Subtract,
    Multiply,
    Divide,
    Equal,
    Not,
    Greater,
    Less,
    SetLocal,
    GetLocal,
    GetGlobal,
    SetGlobal,
    Print, // TODO: Temporary opcode.
    Closure,
    Call,
    Return,
    Pop
}