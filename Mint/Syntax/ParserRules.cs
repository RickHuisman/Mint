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
            new(TokenType.True, Parser.Bool, null, Precedence.None),
            new(TokenType.False, Parser.Bool, null, Precedence.None),
            new(TokenType.Plus, null, Parser.Binary, Precedence.Term),
            new(TokenType.Minus, null, Parser.Binary, Precedence.Term),
            new(TokenType.Star, null, Parser.Binary, Precedence.Factor),
            new(TokenType.Slash, null, Parser.Binary, Precedence.Factor),
            new(TokenType.EqualEqual, null, Parser.Binary, Precedence.Equality),
            new(TokenType.BangEqual, null, Parser.Binary, Precedence.Equality),
            new(TokenType.GreaterThan, null, Parser.Binary, Precedence.Comparison),
            new(TokenType.GreaterThanEqual, null, Parser.Binary, Precedence.Comparison),
            new(TokenType.LessThan, null, Parser.Binary, Precedence.Comparison),
            new(TokenType.LessThanEqual, null, Parser.Binary, Precedence.Comparison),
            new(TokenType.LeftParen, null, Parser.ParseCall, Precedence.Call),
            new(TokenType.RightParen, null, null, Precedence.None),
            new(TokenType.Do, null, null, Precedence.None),
            new(TokenType.Print, null, null, Precedence.None),
            new(TokenType.Return, null, null, Precedence.None),
            new(TokenType.Function, null, null, Precedence.None),
            new(TokenType.Name, Parser.ParseName, null, Precedence.None),
            new(TokenType.Local, null, null, Precedence.None),
            new(TokenType.End, null, null, Precedence.None),
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
    Assign, // =
    Equality, // == !=
    Comparison, // >= <= > <
    Term, // + -
    Factor, // * / %
    Call, // foo()
}