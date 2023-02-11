namespace Mint.Syntax;

public class Parser
{
    private static List<Token> _tokens;

    public Chunk ParseChunk(List<Token> tokens)
    {
        _tokens = tokens;
        _tokens.Reverse();

        var block = ParseBlock();
        return new Chunk(block);
    }

    private static Block ParseBlock()
    {
        var returnStatement = new ReturnStatement(null);

        var statements = new List<IStatement>();
        while (HasNext())
        {
            var statement = ParseStatement();
            if (statement is ReturnStatement _return)
            {
                returnStatement = _return;
            }
            else
            {
                statements.Add(statement);
            }
        }

        return new Block(statements, returnStatement);
    }

    private static IStatement ParseStatement()
    {
        switch (PeekType())
        {
            case TokenType.Do:
                return ParseBlock2();
            case TokenType.Return:
                return ParseReturn();
            case TokenType.Print:
                return ParsePrint();
            case TokenType.Local:
                // TODO: Check for local function.
                return ParseLocalStatement();
            case TokenType.Function:
                return ParseFunctionStatement();
            case TokenType.Name:
                // Check if next token is a '='.
                if (PeekType(2) == TokenType.Equal)
                {
                    return ParseAssignmentStatement();
                }

                break;
        }

        return new ExpressionStatement(ParseExpression());
    }

    private static IStatement ParseReturn()
    {
        Consume(TokenType.Return, "");
        // TODO: Check if has expression.
        var expr = ParseExpression();
        return new ReturnStatement(expr);
    }

    private static IStatement ParseBlock2()
    {
        Consume(TokenType.Do, "");
        var statements = new List<IStatement>();
        while (!Match(TokenType.End))
        {
            var statement = ParseStatement();
            statements.Add(statement);
        }

        // TODO: Parse return.
        return new Block(statements, null);
    }

    private static IStatement ParsePrint()
    {
        Consume(TokenType.Print, "");
        Consume(TokenType.LeftParen, "");
        var value = ParseExpression();
        Consume(TokenType.RightParen, "");
        return new PrintStatement(value);
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
        var right = ParsePrecedence(rule.Precedence);
        var op = token.Type switch
        {
            TokenType.Plus => BinaryOperator.Add,
            TokenType.Minus => BinaryOperator.Subtract,
            TokenType.Star => BinaryOperator.Multiply,
            TokenType.Slash => BinaryOperator.Divide,
            TokenType.EqualEqual => BinaryOperator.Equal,
            TokenType.BangEqual => BinaryOperator.NotEqual,
            TokenType.GreaterThan => BinaryOperator.Greater,
            TokenType.GreaterThanEqual => BinaryOperator.GreaterThanEqual,
            TokenType.LessThan => BinaryOperator.Less,
            TokenType.LessThanEqual => BinaryOperator.LessThanEqual,
            _ => throw new NotImplementedException()
        };
        return new BinaryExpression(left, op, right);
    }

    private static IStatement ParseAssignmentStatement()
    {
        var name = Consume(TokenType.Name, "");
        Consume(TokenType.Equal, "");
        var value = ParseExpression();
        return new AssignmentStatement(name.Source, value);
    }

    public static IExpression ParseCall(Token token, IExpression left)
    {
        Consume(TokenType.RightParen, "TODO");
        return new FunctionCallExpression(left, new List<IExpression>());
    }

    public static IExpression ParseName(Token token)
    {
        // TODO: Fix.
        if (Check(TokenType.LeftParen))
        {
            Consume(TokenType.LeftParen, "");
            Consume(TokenType.RightParen, "");
            // Function call.
            return new FunctionCallExpression(
                new NameExpression(token.Source),
                new List<IExpression>()
            );
        }

        return new NameExpression(token.Source);
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

    private static TokenType PeekType(int n) => _tokens[^n].Type;

    private static bool HasNext() => _tokens.Any();
}