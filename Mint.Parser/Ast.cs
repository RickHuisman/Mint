namespace Mint.Parser;

public record Chunk(Block Block);

public interface IStatement
{
}

public record Block(List<IStatement> Statements, ReturnStatement? ReturnStatement) : IStatement;

public record PrintStatement(IExpression Value) : IStatement;

public record AssignmentStatement(string Name, IExpression Value) : IStatement;

public record LocalStatement(string Name, IExpression Value) : IStatement;

public record FunctionStatement(string Name, List<string> Parameters, Block Body) : IStatement;

public record FunctionCallExpression(IExpression Callee, List<IExpression> Arguments) : IExpression;

public record ReturnStatement(IExpression? Expression) : IStatement;

public record ExpressionStatement(IExpression Expression) : IStatement;

public interface IExpression
{
}

public record BinaryExpression(IExpression Left, BinaryOperator Operator, IExpression Right) : IExpression;

public record UnaryExpression(UnaryOperator Operator, IExpression Left) : IExpression;

public record NumberExpression(double Number) : IExpression;

public record BoolExpression(bool Bool) : IExpression;

public record NilExpression : IExpression;

public record NameExpression(string Name) : IExpression;

public record AndExpression(IExpression Left, IExpression Right) : IExpression;

public record OrExpression(IExpression Left, IExpression Right) : IExpression;

public record IfElseStatement(IExpression Condition, Block Then, Block? Else) : IStatement;

public enum BinaryOperator
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Equal,
    NotEqual,
    Greater,
    GreaterThanEqual,
    Less,
    LessThanEqual,
    And,
    Or
}

public enum UnaryOperator
{
    Not,
    Negate
}