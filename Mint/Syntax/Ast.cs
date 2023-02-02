using Mint.Compiler;
using Mint.VM;
using ValueType = Mint.Compiler.ValueType;

namespace Mint.Syntax;

public record Chunk(Block Block);

public record Block(List<IStatement> Statements, ReturnStatement? ReturnStatement);

public interface IStatement
{
    public void Compile(Compiler.Compiler compiler);
}

public record GlobalStatement(string Name, IExpression Value) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        throw new NotImplementedException();
    }
}

public record LocalStatement(string Name, IExpression Value) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        throw new NotImplementedException();
    }
}

public record FunctionStatement(string Name, List<string> Parameters, Block Body) : IStatement
{
    public void Compile(Compiler.Compiler compiler)
    {
        throw new NotImplementedException();
    }
}

public record FunctionCallExpression(IExpression Name, List<IExpression> Arguments) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        throw new NotImplementedException();
    }
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
        switch (Operator)
        {
            case BinaryOperator.Add:
                compiler.Emit(Opcode.Add);
                break;
            case BinaryOperator.Equal:
                compiler.Emit(Opcode.Equal);
                break;
            case BinaryOperator.NotEqual:
                compiler.Emit(Opcode.NotEqual);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public record UnaryExpression(UnaryOperator Operator, IExpression Left) : IExpression
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
        compiler.AddConstant(new Value(ValueType.Number, Number));
        compiler.Emit(Opcode.LoadConstant);
    }
}

public record NameExpression(string Name) : IExpression
{
    public void Compile(Compiler.Compiler compiler)
    {
        throw new NotImplementedException();
    }
}

public enum BinaryOperator
{
    Add,
    Equal,
    NotEqual
}

public enum UnaryOperator
{
}