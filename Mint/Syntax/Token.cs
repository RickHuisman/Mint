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

    // Literals
    Identifier,
    String,
    Number,

    // Reserved keywords
    // TODO

    Eof
}

public static class KeywordTranslator
{
    public static TokenType FromString(string str)
    {
        return str switch
        {
            _ => TokenType.Identifier
        };
    }
}