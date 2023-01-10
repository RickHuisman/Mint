namespace Mint.Syntax;

public static class ParserRules
{
    public static ParseRule GetRule(TokenType type)
    {
        var rule = GetRules().SingleOrDefault(r => r.Type == type);
        if (rule == null)
        {
            throw new Exception($"No matching rule found for TokenType: {type}");
        }

        return rule;
    }

    private static IEnumerable<ParseRule> GetRules()
    {
        return new List<ParseRule>
        {
            new(TokenType.Number, Parser.Number, null, Precedence.None),
            new(TokenType.Plus, null, Parser.Binary, Precedence.Term),
            new(TokenType.Eof, null, null, Precedence.None),
        };
    }
}

public class ParseRule
{
    public readonly TokenType Type;
    public readonly Func<Token, IExpression>? Prefix;
    public readonly Func<Token, IExpression, IExpression>? Infix;
    public readonly Precedence Precedence;

    public ParseRule(
        TokenType type,
        Func<Token, IExpression>? prefix,
        Func<Token, IExpression, IExpression>? infix,
        Precedence precedence)
    {
        Type = type;
        Prefix = prefix;
        Infix = infix;
        Precedence = precedence;
    }
}

public enum Precedence
{
    None,
    Term, // + -
    Factor, // * / %
}