using Mint.VM;

namespace Mint.Syntax;

public record Chunk(Block Block);

public record Block(List<IStatement> Statements, ReturnStatement? ReturnStatement);

public interface IStatement
{
    public void Compile(Compiler.Compiler compiler);
}

public record ReturnStatement : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        throw new NotImplementedException();
    }
}

public record ExpressionStatement(IExpression Expression) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        Expression.Compile(compiler);
    }
}

public interface IExpression
{
    public void Compile(Compiler.Compiler compiler);
}

public record BinaryExpression(IExpression Left, BinaryOperator Operator, IExpression Right) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        Left.Compile(compiler);
        Right.Compile(compiler);
        compiler.Emit(Opcode.Add);
    }
}

public record UnaryExpression(BinaryOperator Operator, IExpression Left) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        throw new NotImplementedException();
    }
}

public record NumberExpression(double Number) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        compiler.AddConstant(Number);
        compiler.Emit(Opcode.LoadK);
    }
}

public enum BinaryOperator
{
    Add,
}

public enum UnaryOperator
{
}