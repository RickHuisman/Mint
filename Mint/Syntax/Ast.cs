namespace Mint.Syntax;

public record Chunk(Block Block);

public record Block(List<IStatement> Statements, ReturnStatement? ReturnStatement);

public interface IStatement
{
}

public record ReturnStatement() : IStatement;

public record ExpressionStatement(IExpression Expression) : IStatement;

public interface IExpression
{
}

public record BinaryExpression(IExpression Left, BinaryOperator Operator, IExpression Right) : IExpression;

public record UnaryExpression(BinaryOperator Operator, IExpression Left) : IExpression;

public record NumberExpression(double Number) : IExpression;

public enum BinaryOperator
{
    Add,
}

public enum UnaryOperator
{
}