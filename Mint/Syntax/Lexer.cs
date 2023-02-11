namespace Mint.Syntax;

public static class Lexer
{
    private static int _current;
    private static string _source = "";

    public static List<Token> Lex(string source)
    {
        ResetLexer();
        _source = source;

        var tokens = new List<Token>();

        while (!IsAtEnd())
        {
            var token = ScanToken();
            if (token == null) break;
            tokens.Add(token);
        }

        return tokens;
    }

    private static Token? ScanToken()
    {
        SkipWhitespace();

        if (IsAtEnd()) return null;

        var c = Advance();

        if (char.IsLetter(c)) return Identifier();
        if (char.IsDigit(c)) return Number();

        return c switch
        {
            '(' => MakeToken(TokenType.LeftParen),
            ')' => MakeToken(TokenType.RightParen),
            '{' => MakeToken(TokenType.LeftBrace),
            '}' => MakeToken(TokenType.RightBrace),
            ';' => MakeToken(TokenType.Semicolon),
            ',' => MakeToken(TokenType.Comma),
            '.' => MakeToken(TokenType.Dot),
            '-' => MakeToken(TokenType.Minus),
            '+' => MakeToken(TokenType.Plus),
            '/' => MakeToken(TokenType.Slash),
            '*' => MakeToken(TokenType.Star),
            '%' => MakeToken(TokenType.Percent),
            '!' => MakeToken(TokenType.Bang),
            '~' => MakeToken(Match('=') ? TokenType.TildeEqual : throw new Exception()), // TODO: Fix.
            '=' => MakeToken(Match('=') ? TokenType.EqualEqual : TokenType.Equal),
            '<' => MakeToken(Match('=') ? TokenType.LessThanEqual : TokenType.LessThan),
            '>' => MakeToken(Match('=') ? TokenType.GreaterThanEqual : TokenType.GreaterThan),
            '"' => String(),
            _ => throw new UnexpectedChar(c)
        };
    }

    private static Token Identifier()
    {
        var start = _current - 1;
        while (char.IsLetter(Peek()) || char.IsDigit(Peek())) Advance();

        var identifier = _source[start.._current];
        var identifierType = KeywordTranslator.FromString(identifier);

        return new Token(identifierType, identifier);
    }

    private static Token Number()
    {
        var start = _current - 1;
        while (char.IsDigit(Peek())) Advance();

        // Look for a fractional part
        if (Peek() == '.' && char.IsDigit(PeekNext()))
        {
            // Consume the "."
            Advance();

            while (char.IsDigit(Peek())) Advance();
        }

        return new Token(TokenType.Number, _source[start.._current]);
    }

    private static Token String()
    {
        var start = _current;
        while (Peek() != '"' && !IsAtEnd()) Advance();

        if (IsAtEnd()) throw new UnterminatedStringException();

        var end = _current;

        // The closing quote
        Advance();

        return new Token(TokenType.String, _source[start..end]);
    }

    private static Token MakeToken(TokenType type)
    {
        var c = _source[_current - 1];
        var token = new Token(type, c.ToString());
        return token;
    }

    private static void SkipWhitespace()
    {
        while (true)
        {
            // Check and skip comments.
            if (Peek() == '-' && PeekNext() == '-')
            {
                // Advance till end of line.
                while (Peek() != '\n' && !IsAtEnd()) Advance();
            }

            if (char.IsWhiteSpace(Peek()))
            {
                Advance();
            }
            else return;
        }
    }

    private static bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (_source[_current] != expected) return false;

        Advance();
        return true;
    }

    private static char PeekNext()
    {
        return IsAtEnd() ? '\0' : _source[_current + 1];
    }

    private static char Peek()
    {
        return IsAtEnd() ? '\0' : _source[_current];
    }

    private static char Advance()
    {
        _current += 1;
        return _source[_current - 1];
    }

    private static bool IsAtEnd() => _current >= _source.Length;

    private static void ResetLexer() => _current = 0;
}