namespace Mint.Syntax;

public class Parser
{
    private static List<Token> _tokens;

    public Chunk ParseChunk(List<Token> tokens)
    {
        _tokens = tokens;
        _tokens.Reverse();

        var block = ParseBlock();
        return new(block);
    }

    private Block ParseBlock()
    {
        var statements = new List<IStatement>();

        while (HasNext())
        {
            var statement = ParseStatement();
            statements.Add(statement);
        }

        return new(statements, null);
    }

    private static IStatement ParseStatement()
    {
        switch (PeekType())
        {
            case TokenType.Local:
                // TODO: Check for local function.
                return ParseLocalStatement();
            case TokenType.Function:
                return ParseFunctionStatement();
            default:
                return new ExpressionStatement(ParseExpression());
        }
    }

    private static IStatement ParseLocalStatement()
    {
        Consume(TokenType.Local, "");
        var name = Consume(TokenType.Name, "");
        Consume(TokenType.Equal, "");
        var value = ParseExpression();
        return new LocalStatement(name.Source, value);
    }

    private static IStatement ParseFunctionStatement()
    {
        Consume(TokenType.Function, "");
        var name = Consume(TokenType.Name, "TODO");
        Consume(TokenType.LeftParen, "TODO");
        Consume(TokenType.RightParen, "TODO");
        var statements = new List<IStatement>();
        while (!Check(TokenType.End))
        {
            var statement = ParseStatement();
            statements.Add(statement);
        }
        var block = new Block(statements, null);
        Consume(TokenType.End, "TODO");
        return new FunctionStatement(name.Source, new List<string>(), block);
    }

    private static IExpression ParseExpression()
    {
        return ParsePrecedence(Precedence.None + 1);
    }

    private static IExpression ParsePrecedence(Precedence precedence)
    {
        var token = Next();
        var prefixRule = ParserRules.GetRule(token.Type).Prefix;
        if (prefixRule == null) throw new Exception("Expected expression");

        var expr = prefixRule(token);

        if (HasNext()) // TODO: Cleanup.
        {
            while (precedence <= ParserRules.GetRule(PeekType()).Precedence)
            {
                token = Next();
                var infixRule = ParserRules.GetRule(token.Type).Infix;
                if (infixRule == null) throw new Exception("Expected expression");
                return infixRule(token, expr);
            }
        }

        return expr;
    }

    public static IExpression Binary(Token token, IExpression left)
    {
        var rule = ParserRules.GetRule(token.Type);
        var right = ParsePrecedence(rule.Precedence + 1);
        var op = token.Type switch
        {
            TokenType.Plus => BinaryOperator.Add,
            TokenType.EqualEqual => BinaryOperator.Equal,
            TokenType.BangEqual => BinaryOperator.NotEqual,
            _ => throw new NotImplementedException()
        };
        return new BinaryExpression(left, op, right);
    }

    public static IExpression Number(Token token)
    {
        var number = Convert.ToDouble(token.Source);
        return new NumberExpression(number);
    }

    private static Token Next()
    {
        var popped = _tokens[^1];
        _tokens.RemoveAt(_tokens.Count - 1);
        return popped;
    }

    private static Token Consume(TokenType type, string message)
    {
        if (PeekType() == type) return Next();
        throw new Exception(message);
    }

    private static bool Check(params TokenType[] types)
    {
        return types.Any(t => PeekType() == t);
    }

    private static bool Match(TokenType type)
    {
        if (PeekType() != type) return false;

        Next();
        return true;
    }

    private static TokenType PeekType() => _tokens[^1].Type;

    private static bool HasNext() => _tokens.Any();
}