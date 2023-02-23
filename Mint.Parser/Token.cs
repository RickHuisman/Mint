namespace Mint.Parser;

public record Token(TokenType Type, string Source);

public enum TokenType
{
    // Single-character tokens
    LeftParen,
    RightParen,
    LeftBrace,
    RightBrace,
    Comma,
    Dot,
    Minus,
    Plus,
    Percent,
    Semicolon,
    Star,

    // One or two character tokens
    Bang,
    TildeEqual,
    Equal,
    EqualEqual,
    LessThan,
    LessThanEqual,
    GreaterThan,
    GreaterThanEqual,
    Slash,

    // Reserved keywords
    True,
    False,
    Nil,
    And,
    Or,
    Not,
    Break,
    Do,
    If,
    Then,
    Else,
    ElseIf,
    Goto,
    End,
    Function,
    Local,
    Return,
    Print, // TODO: Temporary keyword.
    
    // Values
    Name,
    String,
    Number
}

public static class KeywordTranslator
{
    public static TokenType FromString(string str)
    {
        return str switch
        {
            "true" => TokenType.True,
            "false" => TokenType.False,
            "nil" => TokenType.Nil,
            "and" => TokenType.And,
            "or" => TokenType.Or,
            "not" => TokenType.Not,
            "break" => TokenType.Break,
            "do" => TokenType.Do,
            "if" => TokenType.If,
            "then" => TokenType.Then,
            "else" => TokenType.Else,
            "elseif" => TokenType.ElseIf,
            "goto" => TokenType.Goto,
            "end" => TokenType.End,
            "function" => TokenType.Function,
            "local" => TokenType.Local,
            "return" => TokenType.Return,
            "print" => TokenType.Print,
            _ => TokenType.Name
        };
    }
}