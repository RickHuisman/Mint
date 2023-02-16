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
    Negate,
    Greater,
    Less,
    SetLocal,
    GetLocal,
    GetGlobal,
    SetGlobal,
    Print, // TODO: Temporary opcode.
    Closure,
    Call,
    Jump,
    JumpIfFalse,
    Return,
    And,
    Or,
    Pop
}