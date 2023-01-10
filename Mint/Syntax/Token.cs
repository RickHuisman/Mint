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
    Comment,

    // Reserved keywords
    Break,
    Do,
    Else,
    ElseIf,
    End,
    Function,
    Goto,
    If,
    
    Name,
    String,
    Number,

    Eof
}

public static class KeywordTranslator
{
    public static TokenType FromString(string str)
    {
        return str switch
        {
            "break" => TokenType.Break,
            "do" => TokenType.Do,
            "else" => TokenType.Else,
            "elseif" => TokenType.ElseIf,
            "end" => TokenType.End,
            "function" => TokenType.Function,
            "goto" => TokenType.Goto,
            "if" => TokenType.If,
            _ => TokenType.Name
        };
    }
}