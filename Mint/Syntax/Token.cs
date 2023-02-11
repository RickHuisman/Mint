namespace Mint.Syntax;

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
    BangEqual,
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
    And,
    Or,
    Not,
    Break,
    Do,
    Else,
    ElseIf,
    End,
    Function,
    Goto,
    If,
    Local,
    Return,
    Print, // TODO: Temporary keyword.
    
    Name,
    String,
    Number,
}

public static class KeywordTranslator
{
    public static TokenType FromString(string str)
    {
        return str switch
        {
            "true" => TokenType.True,
            "false" => TokenType.False,
            "and" => TokenType.And,
            "or" => TokenType.Or,
            "not" => TokenType.Not,
            "break" => TokenType.Break,
            "do" => TokenType.Do,
            "else" => TokenType.Else,
            "elseif" => TokenType.ElseIf,
            "end" => TokenType.End,
            "function" => TokenType.Function,
            "goto" => TokenType.Goto,
            "if" => TokenType.If,
            "local" => TokenType.Local,
            "return" => TokenType.Return,
            "print" => TokenType.Print,
            _ => TokenType.Name
        };
    }
}