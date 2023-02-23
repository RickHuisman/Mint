namespace Mint.Parser;

public class Parser
{
    private static List<Token> _tokens;

    public static Chunk ParseChunk(List<Token> tokens)
    {
        _tokens = tokens;
        _tokens.Reverse();

        var returnStatement = new ReturnStatement(null);
        var statements = new List<IStatement>();
        while (!IsAtEnd())
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

        var block = new Block(statements, returnStatement);
        return new Chunk(block);
    }

    private static IStatement ParseStatement()
    {
        switch (PeekType())
        {
            case TokenType.Do:
                return ParseBlock();
            case TokenType.Return:
                return ParseReturn();
            case TokenType.Print:
                return ParsePrint();
            case TokenType.Local:
                return ParseLocalStatement();
            case TokenType.If:
                return ParseIfElseStatement();
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
        if (IsAtEnd()) return new ReturnStatement(null);
        if (Check(TokenType.End, TokenType.Else, TokenType.ElseIf))
        {
            return new ReturnStatement(null);
        }
        return new ReturnStatement(ParseExpression());
    }

    private static Block ParseBlock()
    {
        Match(TokenType.Do);
        var statements = new List<IStatement>();

        ReturnStatement? returnStatement = null;
        while (!Match(TokenType.End) && !Check(TokenType.Else))
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

    private static IStatement ParseIfElseStatement()
    {
        Consume(TokenType.If, "");
        var condition = ParsePrecedence(Precedence.Assign);
        
        // Then branch.
        Consume(TokenType.Then, "");
        var then = ParseBlock();
        
        // Else branch.
        Block? @else = null;
        if (!IsAtEnd() && Match(TokenType.Else))
        {
            @else = ParseBlock();
        }
        return new IfElseStatement(condition, then, @else);
    }
    
    private static IStatement ParseFunctionStatement()
    {
        Consume(TokenType.Function, "");
        var name = Consume(TokenType.Name, "TODO");
        Consume(TokenType.LeftParen, "TODO");
        Consume(TokenType.RightParen, "TODO");
        var block = ParseBlock();
        return new FunctionStatement(name.Source, new List<string>(), block);
    }

    private static IExpression ParseExpression()
    {
        return ParsePrecedence(Precedence.Assign);
    }

    private static IExpression ParsePrecedence(Precedence precedence)
    {
        var token = Next();
        var prefixRule = GetRule(token.Type).Prefix;
        if (prefixRule == null) throw new Exception("Expected expression");

        var expr = prefixRule(token);

        if (IsAtEnd()) return expr; // TODO: Cleanup.
        
        while (precedence <= GetRule(PeekType()).Precedence)
        {
            token = Next();
            var infixRule = GetRule(token.Type).Infix;
            if (infixRule == null) throw new Exception("Expected expression");
            return infixRule(token, expr);
        }

        return expr;
    }

    public static IExpression Or(Token token, IExpression left)
    {
        var rule = ParserRules.GetRule(token.Type);
        var right = ParsePrecedence(rule.Precedence);
        return new OrExpression(left, right);
    }
    
    public static IExpression And(Token token, IExpression left)
    {
        var rule = ParserRules.GetRule(token.Type);
        var right = ParsePrecedence(rule.Precedence);
        return new AndExpression(left, right);
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
            TokenType.TildeEqual => BinaryOperator.NotEqual,
            TokenType.GreaterThan => BinaryOperator.Greater,
            TokenType.GreaterThanEqual => BinaryOperator.GreaterThanEqual,
            TokenType.LessThan => BinaryOperator.Less,
            TokenType.LessThanEqual => BinaryOperator.LessThanEqual,
            TokenType.And => BinaryOperator.And,
            TokenType.Or => BinaryOperator.Or,
            _ => throw new NotImplementedException()
        };
        return new BinaryExpression(left, op, right);
    }

    public static IExpression Unary(Token token)
    {
        var op = token.Type switch
        {
            TokenType.Not => UnaryOperator.Not,
            TokenType.Minus => UnaryOperator.Negate,
            _ => throw new ArgumentOutOfRangeException()
        };
        var expr = ParsePrecedence(Precedence.Unary);
        return new UnaryExpression(op, expr);
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
        // Function call.
        if (Check(TokenType.LeftParen)) 
        {
            Consume(TokenType.LeftParen, "");
            Consume(TokenType.RightParen, "");
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

    public static IExpression Bool(Token token)
    {
        var boolean = Convert.ToBoolean(token.Source);
        return new BoolExpression(boolean);
    }

    public static IExpression Nil(Token token)
    {
        return new NilExpression();
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

    private static ParseRule GetRule(TokenType tokenType) => ParserRules.GetRule(tokenType);
    
    private static bool IsAtEnd() => !_tokens.Any();
}